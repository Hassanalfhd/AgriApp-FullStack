using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Agricultural_For_CV_DAL.Entities;
using Agricultural_For_CV_DAL.Interfaces;
using Agricultural_For_CV_Shared.Dtos.CropsDtos;
using Agricultural_For_CV_Shared.Interfaces;
using Agricultural_For_CV_Shared.Results;
using Agricultural_For_CV_Shared.Settings;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace Agricultural_For_CV_BLL.Services
{
    public class CropService : ICropService
    {
        private readonly ICropRepository _cropRepository;
        private readonly IImageService _imageService;
        private readonly AppSettings _appSettings;
        private readonly ILogger<CropService> _logger;

        public CropService(
            ICropRepository cropRepository,
            IImageService imageService,
            IOptions<AppSettings> appSettings,
            ILogger<CropService> logger)
        {
            _cropRepository = cropRepository;
            _imageService = imageService;
            _appSettings = appSettings.Value;
            _logger = logger;
        }

        private Result<IEnumerable<CropResponseDto>> CropDataResponse(IEnumerable<Crop> crops)
        {
            return Result<IEnumerable<CropResponseDto>>.Success(
                crops.Select(c => new CropResponseDto
                {
                    Id = c.Id,
                    Name = c.Name,
                    CategoryId = c.CategoryId,
                    OwnerId = c.OwnerId,
                    CreatedAt = c.CreatedAt,
                    ImagePath = c.ImagePath,
                    Username = c.Owner?.Username,
                    CategoryName = c.Category?.Name
                }));
        }

        public async Task<Result<IEnumerable<CropResponseDto>>> GetAllAsync()
        {
            try
            {
                var crops = await _cropRepository.GetAllAsync(true);
                _logger.LogInformation("All crops retrieved successfully. Count: {Count}", crops.Count());
                return CropDataResponse(crops);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to retrieve all crops.");
                return Result<IEnumerable<CropResponseDto>>.Failure("An error occurred while retrieving crops.");
            }
        }

        public async Task<Result<IEnumerable<CropResponseDto>>> GetCropsByUserIdAsync(int userId)
        {
            if (userId <= 0)
            {
                _logger.LogWarning("Invalid user ID: {UserId}", userId);
                return Result<IEnumerable<CropResponseDto>>.Failure("Invalid user ID.");
            }

            try
            {
                var crops = await _cropRepository.GetCropsByUserIdAsync(userId);
                if (crops == null || !crops.Any())
                {
                    _logger.LogInformation("No crops found for user ID: {UserId}", userId);
                    return Result<IEnumerable<CropResponseDto>>.Failure("No crops found for this user.");
                }

                _logger.LogInformation("Crops retrieved for user ID: {UserId}", userId);
                return CropDataResponse(crops);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to retrieve crops for user ID: {UserId}", userId);
                return Result<IEnumerable<CropResponseDto>>.Failure("An error occurred while retrieving crops.");
            }
        }

        public async Task<Result<IEnumerable<CropResponseDto>>> GetCropsByCategoryIdAsync(int categoryId)
        {
            if (categoryId <= 0)
            {
                _logger.LogWarning("Invalid category ID: {CategoryId}", categoryId);
                return Result<IEnumerable<CropResponseDto>>.Failure("Invalid category ID.");
            }

            try
            {
                var crops = await _cropRepository.GetCropsByCategoryIdAsync(categoryId);
                if (crops == null || !crops.Any())
                {
                    _logger.LogInformation("No crops found for category ID: {CategoryId}", categoryId);
                    return Result<IEnumerable<CropResponseDto>>.Failure("No crops found for this category.");
                }

                _logger.LogInformation("Crops retrieved for category ID: {CategoryId}", categoryId);
                return CropDataResponse(crops);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to retrieve crops for category ID: {CategoryId}", categoryId);
                return Result<IEnumerable<CropResponseDto>>.Failure("An error occurred.");
            }
        }

        public async Task<Result<IEnumerable<CropResponseDto>>> SearchCropsByNameAsync(string name)
        {
            if (string.IsNullOrWhiteSpace(name))
            {
                _logger.LogWarning("Invalid crop name provided for search.");
                return Result<IEnumerable<CropResponseDto>>.Failure("Crop name cannot be empty.");
            }

            try
            {
                var crops = await _cropRepository.SearchCropsByNameAsync(name);
                if (crops == null || !crops.Any())
                {
                    _logger.LogInformation("No crops found matching the name: {Name}", name);
                    return Result<IEnumerable<CropResponseDto>>.Failure("No crops found with this name.");
                }

                _logger.LogInformation("Crops found matching the name: {Name}", name);
                return CropDataResponse(crops);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error searching crops by name: {Name}", name);
                return Result<IEnumerable<CropResponseDto>>.Failure("An error occurred.");
            }
        }

        public async Task<Result<CropResponseDto>> GetByIdAsync(int id)
        {
            if (id <= 0)
            {
                _logger.LogWarning("Invalid crop ID: {Id}", id);
                return Result<CropResponseDto>.Failure("Invalid crop ID.");
            }

            try
            {
                var crop = await _cropRepository.GetByIdAsync(id, true);
                if (crop == null)
                {
                    _logger.LogWarning("Crop not found with ID: {Id}", id);
                    return Result<CropResponseDto>.Failure("Crop not found.");
                }

                _logger.LogInformation("Crop retrieved successfully with ID: {Id}", id);
                return Result<CropResponseDto>.Success(new CropResponseDto
                {
                    Id = crop.Id,
                    Name = crop.Name,
                    ImagePath = crop.ImagePath,
                    CategoryId = crop.CategoryId,
                    OwnerId = crop.OwnerId,
                    CreatedAt = crop.CreatedAt,
                    Username = crop.Owner?.Username,
                    CategoryName = crop.Category?.Name
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving crop with ID: {Id}", id);
                return Result<CropResponseDto>.Failure("An error occurred.");
            }
        }

        public async Task<Result<bool>> IsCropExist(int id)
        {
            if (id <= 0)
            {
                _logger.LogWarning("Invalid crop ID: {Id}", id);
                return Result<bool>.Failure("Invalid crop ID.");
            }

            try
            {
                bool exists = await _cropRepository.IsCropExist(id);
                _logger.LogInformation("Checked existence for crop ID: {Id} - Exists: {Exists}", id, exists);
                return exists
                    ? Result<bool>.Success(true)
                    : Result<bool>.Failure("Crop not found.");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error checking crop existence with ID: {Id}", id);
                return Result<bool>.Failure("An error occurred.");
            }
        }

        public async Task<Result<CropResponseDto>> CreateAsync(CreateCropDto dto)
        {
            if (dto == null || string.IsNullOrWhiteSpace(dto.Name))
            {
                _logger.LogWarning("Invalid crop creation data provided.");
                return Result<CropResponseDto>.Failure("Invalid crop data.");
            }

            var crop = new Crop
            {
                Name = dto.Name.Trim(),
                CategoryId = dto.CategoryId,
                OwnerId = dto.OwnerId
            };

            var result = await _imageService.SaveImageAsync(dto.ImagePath, _appSettings.ImagePaths.Crops);

            if (!result.IsSuccess)
            {
                // فشل الحفظ، يمكنك إرجاع خطأ أو تسجيله
                return Result<CropResponseDto>.Failure(result.Message);
            }

            crop.ImagePath = result.Data;


            try
            {
                await _cropRepository.AddAsync(crop);
                _logger.LogInformation("Crop created successfully: {Name}", crop.Name);
            }
            catch (Exception ex)
            {
                _imageService.DeleteImage(crop.ImagePath);
                _logger.LogError(ex, "Failed to create crop: {Name}", dto.Name);
                return Result<CropResponseDto>.Failure("An error occurred while creating crop.");
            }

            return Result<CropResponseDto>.Success(new CropResponseDto
            {
                Id = crop.Id,
                Name = crop.Name,
                ImagePath = crop.ImagePath,
                CategoryId = crop.CategoryId,
                OwnerId = crop.OwnerId,
                CreatedAt = crop.CreatedAt
            });
        }

        public async Task<Result<bool>> DeleteAsync(int id)
        {
            var crop = await _cropRepository.GetByIdAsync(id);
            if (crop == null)
            {
                _logger.LogWarning("Attempted to delete non-existent crop with ID: {Id}", id);
                return Result<bool>.Failure("Crop not found.");
            }

            try
            {
                await _cropRepository.DeleteAsync(id);
                _imageService.DeleteImage(_imageService.GetFullUrl(crop.ImagePath));
                _logger.LogInformation("Crop deleted successfully: {Id}", id);
                return Result<bool>.Success(true);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error deleting crop with ID: {Id}", id);
                return Result<bool>.Failure("An error occurred while deleting crop.");
            }
        }

        public async Task<Result<CropResponseDto>> UpdateAsync(UpdateCropDto dto)
        {
            if (dto == null || dto.Id <= 0)
            {
                _logger.LogWarning("Invalid crop update data.");
                return Result<CropResponseDto>.Failure("Invalid data.");
            }

            var crop = await _cropRepository.GetByIdAsync(dto.Id);
            if (crop == null)
            {
                _logger.LogWarning("Crop not found for update: {Id}", dto.Id);
                return Result<CropResponseDto>.Failure("Crop not found.");
            }

            crop.Name = dto.Name.Trim();
            crop.CategoryId = dto.CategoryId;
            crop.OwnerId = dto.OwnerId;

            // 🔸 Handle image upload
            var result = await _imageService.SaveImageAsync(dto.ImagePath, _appSettings.ImagePaths.Crops);
            if (!result.IsSuccess)
            {
                _logger.LogWarning("Failed to save image: {Error}", result.Message);
            }
            crop.ImagePath = result.Data;

            try
            {
                await _cropRepository.UpdateAsync(crop);
                _logger.LogInformation("Crop updated successfully: {Id}", dto.Id);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error updating crop: {Id}", dto.Id);
                return Result<CropResponseDto>.Failure("An error occurred while updating crop.");
            }

            return Result<CropResponseDto>.Success(new CropResponseDto
            {
                Id = crop.Id,
                Name = crop.Name,
                CreatedAt = crop.CreatedAt,
                OwnerId = crop.OwnerId,
                CategoryId = crop.CategoryId,
                Username = crop.Owner?.Username,
                CategoryName = crop.Category?.Name,
                ImagePath = crop.ImagePath
            });
        }
    }
}
