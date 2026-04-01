using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Agricultural_For_CV_Shared.Dtos.CategoriesDtos;
using Agricultural_For_CV_Shared.Results;

namespace Agricultural_For_CV_Shared.Interfaces
{
    public interface ICategoryService
    {

        Task<Result<IEnumerable<CategoryResponseDto>>> GetAllAsync();
        Task<Result<CategoryResponseDto>> GetById(int id);
        Task<Result<CategoryResponseDto>> AddAsync(CreateCategoryDto dto);
        Task<Result<CategoryResponseDto>> UpdateAsync(UpdateCategoryDto dto);
        Task<Result<bool>> DeleteAsync(int id);


    }
}
