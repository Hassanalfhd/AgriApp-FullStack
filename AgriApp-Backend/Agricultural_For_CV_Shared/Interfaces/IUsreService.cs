using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Agricultural_For_CV_Shared.Dtos.Products;
using Agricultural_For_CV_Shared.Dtos.UserDtos;
using Agricultural_For_CV_Shared.Results;

namespace Agricultural_For_CV_Shared.Interfaces
{
    public interface IUserService
    {

        Task<Result<IEnumerable<UserResponseDto>>> GetAllAsync();

        Task<Result<PagedResult<UserResponseDto>>> GetPagedUsersAsync(int page, int pageSize);
        Task<Result<UserResponseDto>> GetByIdAsync(int id);

        Task<Result<UserResponseDto>> UpdateAsync(UserUpdateDto dto);

        Task<Result<bool>> DeleteAsync(int id);

        Task<Result<bool>> ExistsByEmailAsync(string email);
        Task<Result<bool>> SetUserAccountStatusAsync(int userId, bool isActive);
    
    }
}
