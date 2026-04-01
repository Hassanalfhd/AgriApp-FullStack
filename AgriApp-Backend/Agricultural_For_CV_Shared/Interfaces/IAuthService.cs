using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Agricultural_For_CV_Shared.Dtos.UserDtos;
using Agricultural_For_CV_Shared.Results;

namespace Agricultural_For_CV_Shared.Interfaces
{
    public interface IAuthService
    {
        Task<Result<UserResponseDto>> RegisterAsync(UserDto dto);
        Task<Result<TokenResponse>> LoginAsync(LoginDto dto);

        //Task<Result<TokenResponse>> RefreshTokenAsync(RefreshTokenDto dto);
        Task<Result<TokenResponse>> RefreshTokenAsync(string expiredToken, string refreshToken);

        Task<Result<bool>> LogoutAsync(RefreshTokenDto dto);
    }
}
