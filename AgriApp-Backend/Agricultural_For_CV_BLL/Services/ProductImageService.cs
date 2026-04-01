using Agricultural_For_CV_BLL.Interfaces;
using Agricultural_For_CV_DAL.Entities;
using Agricultural_For_CV_DAL.Interfaces;
using Agricultural_For_CV_Shared;
using Agricultural_For_CV_Shared.Dtos.ProductsImages;
using Agricultural_For_CV_Shared.Interfaces;
using Agricultural_For_CV_Shared.Results;
using Agricultural_For_CV_Shared.Settings;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace Agricultural_For_CV_BLL.Services
{
    public class ProductImageService : IProductImageService
    {
        private readonly IProductRepository _productRepo;
        private readonly IProductImageRepository _imageRepo;
        private readonly IImageService _imageService;
        private readonly AppSettings _settings;
        private readonly ILogger<ProductImageService> _logger;

        public ProductImageService(
            IProductRepository productRepo,
            IProductImageRepository imageRepo,
            IImageService imageService,
            IOptions<AppSettings> settings,
            ILogger<ProductImageService> logger)
        {
            _productRepo = productRepo;
            _imageRepo = imageRepo;
            _imageService = imageService;
            _settings = settings.Value;
            _logger = logger;
        }

        public async Task<Result<List<ProductImageResponseDto>>> GetImagesAsync(int productId)
        {
            var images = await _imageRepo.GetByProductIdAsync(productId);
            var mapped = images.Select(i => new ProductImageResponseDto
            {
                Id = i.Id,
                ImagePath = i.ImagePath ?? "",
                ImageOrder = i.ImageOrder
            }).ToList();

            return Result<List<ProductImageResponseDto>>.Success(mapped);
        }

        public async Task<Result<ProductImageResponseDto>> AddImageAsync(int productId, IFormFile file, int imageOrder = 0)
        {
            var product = await _productRepo.GetAsync(productId);
            if (product == null)
                return Result<ProductImageResponseDto>.Failure("Product not found.");

            var result = await _imageService.SaveImageAsync(file, _settings.ImagePaths.Products);
            if (!result.IsSuccess)
                return Result<ProductImageResponseDto>.Failure(result.Message);

            var image = new ProductsImages
            {
                ProductId = productId,
                ImagePath = result.Data!,
                ImageOrder = imageOrder == 0 ? (product.ProductsImages?.Count ?? 0) + 1 : imageOrder
            };

            await _imageRepo.AddAsync(image);
            await _imageRepo.SaveChangesAsync();

            return Result<ProductImageResponseDto>.Success(new ProductImageResponseDto
            {
                Id = image.Id,
                ImagePath = image.ImagePath,
                ImageOrder = image.ImageOrder
            }, "Image added successfully.");
        }

        public async Task<Result<List<ProductImageResponseDto>>> AddMultipleImagesAsync(int productId, IFormFile[] files)
        {
            var product = await _productRepo.GetAsync(productId);
            if (product == null)
                return Result<List<ProductImageResponseDto>>.Failure("Product not found.");

            int order = (product.ProductsImages?.Count ?? 0) + 1;
            var addedImages = new List<ProductImageResponseDto>();


            foreach (var file in files)
            {
                var result = await _imageService.SaveImageAsync(file, _settings.ImagePaths.Products);
                if (!result.IsSuccess)
                {
                    _logger.LogWarning("Failed to save image: {Error}", result.Message);
                    continue;
                }

                var image = new ProductsImages
                {
                    ProductId = productId,
                    ImagePath = result.Data!,
                    ImageOrder = order++
                };

                await _imageRepo.AddAsync(image);
                addedImages.Add(new ProductImageResponseDto
                {
                    Id = image.Id,
                    ImagePath = image.ImagePath,
                    ImageOrder = image.ImageOrder
                });
            }

            await _imageRepo.SaveChangesAsync();
            return Result<List<ProductImageResponseDto>>.Success(addedImages, "Images uploaded successfully.");
        }

        public async Task<Result<bool>> DeleteImageAsync(int imageId)
        {
            var image = await _imageRepo.GetByIdAsync(imageId);
            if (image == null)
                return Result<bool>.Failure("Image not found.");

            try
            {
                _imageService.DeleteImage(image.ImagePath);
                await _imageRepo.DeleteAsync(image);
                await _imageRepo.SaveChangesAsync();
                return Result<bool>.Success(true, "Image deleted successfully.");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to delete image {Id}", imageId);
                return Result<bool>.Failure("Failed to delete image.");
            }
        }

    }
}
