using Agricultural_For_CV_Shared.Dtos.ProductsImages;
using Agricultural_For_CV_Shared;
using Agricultural_For_CV_Shared.Results;
using Microsoft.AspNetCore.Http;

namespace Agricultural_For_CV_BLL.Interfaces
{
    public interface IProductImageService
    {
        Task<Result<List<ProductImageResponseDto>>> GetImagesAsync(int productId);
        Task<Result<ProductImageResponseDto>> AddImageAsync(int productId, IFormFile file, int imageOrder = 0);
        Task<Result<List<ProductImageResponseDto>>> AddMultipleImagesAsync(int productId, IFormFile[] files);
        Task<Result<bool>> DeleteImageAsync(int imageId);
    }
}
