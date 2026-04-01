using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Agricultural_For_CV_Shared.Dtos.UserDtos;
using Agricultural_For_CV_Shared.Interfaces;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Microsoft.Extensions.Options;
using Agricultural_For_CV_Shared.Settings;
using System.Security.Cryptography;



namespace Agricultural_For_CV_BLL.Services
{
    public class JwtService : IJwtService
    {

        private readonly string _secretKey;
        private readonly string _audience;
        private readonly string _issuer;
        private readonly int _expiryMinutes;
        public JwtService(IOptions<AppSettings>appSetting)
        {
            _secretKey = appSetting.Value.SecretKey;
            _audience = appSetting.Value.Audience;
            _issuer = appSetting.Value.Issuer;
            _expiryMinutes = appSetting.Value.JwtExpiryMinutes;
        }

        public string GetEmailFromExpiredToken(string token)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var principal = tokenHandler.ReadJwtToken(token);
            return principal.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Email)?.Value;
        }



        public string GenerateToken(JwtUserDto user)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_secretKey);



            var claims = new[]
            {
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new Claim(ClaimTypes.Name, user.Username),
                new Claim(ClaimTypes.Role, user.UserType!.ToString()),
                new Claim(ClaimTypes.Email, user.email)
            };


            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                Expires = DateTime.UtcNow.AddMinutes(_expiryMinutes),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature),
                Audience = _audience,
                Issuer = _issuer,   

            };
            




            var token = tokenHandler.CreateToken(tokenDescriptor);

            return tokenHandler.WriteToken(token);

        }

        public  string GenerateRefreshToken()
        {
            var bytes = new byte[64];
            using var rng = RandomNumberGenerator.Create();
            rng.GetBytes(bytes);
            return Convert.ToBase64String(bytes);

        }
        
        
        public ClaimsPrincipal ValidateToken(string token)
        {
            try
            {
                var tokenHandler = new JwtSecurityTokenHandler();
                var key = Encoding.ASCII.GetBytes(_secretKey);

                var parameters = new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(key),
                    ValidateIssuer = false,
                    ValidateAudience = false,
                    ClockSkew = TimeSpan.Zero
                };

                // 4. عملية التحقق واستخراج المعلومات (تعيد الـ ClaimsPrincipal جاهزاً)
                var principal = tokenHandler.ValidateToken(token, parameters, out SecurityToken validatedToken);

                // التحقق من أن التوكن يتبع معيار JWT فعلياً وليس شيئاً آخر
                if (validatedToken is not JwtSecurityToken jwtSecurityToken ||
                    !jwtSecurityToken.Header.Alg.Equals(SecurityAlgorithms.HmacSha256, StringComparison.InvariantCultureIgnoreCase))
                {
                    return null;
                }


                return principal; // نعيد الـ principal الذي يحتوي على كل الـ Claims (ID, Name, Role)
            }
            catch
            {
                return null; // في حال فشل التحقق، نعيد نل
            }
        }
   
    
    }
}
