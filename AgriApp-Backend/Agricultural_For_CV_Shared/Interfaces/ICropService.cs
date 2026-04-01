using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Agricultural_For_CV_Shared.Dtos.CropsDtos;
using Agricultural_For_CV_Shared.Results;

namespace Agricultural_For_CV_Shared.Interfaces
{
    public interface ICropService
    {
        Task<Result<CropResponseDto>> CreateAsync(CreateCropDto dto);
        Task<Result<CropResponseDto>> UpdateAsync(UpdateCropDto dto);
        Task<Result<bool>> IsCropExist(int id);
        Task<Result<CropResponseDto>> GetByIdAsync(int id);
        Task<Result<IEnumerable<CropResponseDto>>> GetAllAsync();
        Task<Result<bool>> DeleteAsync(int id);
        Task<Result<IEnumerable<CropResponseDto>>> GetCropsByUserIdAsync(int userId);
        Task<Result<IEnumerable<CropResponseDto>>> GetCropsByCategoryIdAsync(int categoryId);

        Task<Result<IEnumerable<CropResponseDto>>> SearchCropsByNameAsync(string name);
    }
}
