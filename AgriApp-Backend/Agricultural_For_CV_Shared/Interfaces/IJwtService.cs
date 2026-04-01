using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Agricultural_For_CV_Shared.Dtos.UserDtos;

namespace Agricultural_For_CV_Shared.Interfaces
{
    public interface IJwtService
    {
        string GenerateToken(JwtUserDto user);
        string GetEmailFromExpiredToken(string token);
        string GenerateRefreshToken();
        ClaimsPrincipal ValidateToken(string token);
    }
}
