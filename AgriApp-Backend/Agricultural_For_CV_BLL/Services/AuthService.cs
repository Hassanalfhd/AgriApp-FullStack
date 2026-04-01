using System;
using System.Linq;
using System.Runtime;
using System.Threading.Tasks;
using Agricultural_For_CV_DAL.Entities;
using Agricultural_For_CV_DAL.Interfaces;
using Agricultural_For_CV_Shared.Dtos.UserDtos;
using Agricultural_For_CV_Shared.Enums;
using Agricultural_For_CV_Shared.Interfaces;
using Agricultural_For_CV_Shared.Results;
using Agricultural_For_CV_Shared.Settings;
using Azure.Core;
using BCrypt.Net;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace Agricultural_For_CV_BLL.Services
{
    public class AuthService : IAuthService
    {
        private readonly IJwtService _jwtService;
        private readonly IUserRepository _userRepository;
        private readonly IImageService _imageService;
        private readonly ILogger<AuthService> _logger;

        public AuthService(
            IUserRepository userRepository,
            IJwtService jwtService,
            IImageService imageService,
            ILogger<AuthService> logger)
        {
            _userRepository = userRepository;
            _jwtService = jwtService;
            _imageService = imageService;
            _logger = logger;
        }

        // Register a new user
        public async Task<Result<UserResponseDto>> RegisterAsync(UserDto dto)
        {
            try
            {
                // 🔸 Basic validation
                if (string.IsNullOrWhiteSpace(dto.Email))
                    return Result<UserResponseDto>.Failure("Email is required.");

                if (string.IsNullOrWhiteSpace(dto.Username))
                    return Result<UserResponseDto>.Failure("Username is required.");

                if (string.IsNullOrWhiteSpace(dto.fullName))
                    return Result<UserResponseDto>.Failure("Full name is required.");

                if (string.IsNullOrWhiteSpace(dto.Password) || dto.Password.Length < 6)
                    return Result<UserResponseDto>.Failure("Password must be at least 6 characters long.");

                // 🔸 Check if email or username already exists
                if (await _userRepository.ExistsByEmailAsync(dto.Email))
                    return Result<UserResponseDto>.Failure("Email already taken.");

                if (await _userRepository.ExistsByUsernameAsync(dto.Username))
                    return Result<UserResponseDto>.Failure("Username already taken.");

                // 🔸 Create new user
                var user = new User
                {
                    fullName = dto.fullName,
                    Username = dto.Username,
                    Email = dto.Email,
                    PasswordHash = BCrypt.Net.BCrypt.HashPassword(dto.Password),
                    UserType = dto.UserType
                };
                 
                
           // 🔸 Save user
           await _userRepository.AddAsync(user);
                _logger.LogInformation("User {UserId} registered successfully with username {Username}.", user.Id, user.Username);

                // 🔸 Response
                var response = new UserResponseDto
                {
                    Id = user.Id,
                    fullName = user.fullName,
                    Username = user.Username,
                    Email = user.Email,
                    UserType = ((UserRole)user.UserType).ToString(),
                };


                return Result<UserResponseDto>.Success(response);


            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred during user registration.");
                return Result<UserResponseDto>.Failure("Failed to register user. Please try again.");
        
            }
        }

        //  Login with token
        public async Task<Result<TokenResponse>> LoginAsync(LoginDto dto)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(dto.Email) || string.IsNullOrWhiteSpace(dto.Password))
                    return Result<TokenResponse>.Failure("Email and password are required.");

                var user = await _userRepository.GetByUsernameOrEmailAsync(dto.Email);

                if (user == null)
                {
                    _logger.LogWarning("Login failed: No user found with email or username {Email}.", dto.Email);
                    return Result<TokenResponse>.Failure("Login failed: Invalid credentials.");
                }

                if (!user.isActive)
                {

                    _logger.LogWarning("Login failed: user  with email or username {Email} is unActive.", dto.Email);
                    return Result<TokenResponse>.Failure($"Login failed: user  with email or username ${dto.Email} is unActive.");
                }
                


                bool isPasswordValid = BCrypt.Net.BCrypt.Verify(dto.Password, user.PasswordHash);
                if (!isPasswordValid)
                {
                    _logger.LogWarning("Login failed: Invalid password for user {UserId}.", user.Id);
                    return Result<TokenResponse>.Failure("Login failed:Invalid credentials.");
                }


                var jwtUser = new JwtUserDto
                {
                    Id = user.Id,
                    Username = user.Username,
                    UserType = ((UserRole)user.UserType).ToString(),
                    email = user.Email,
                };



                var token = _jwtService.GenerateToken(jwtUser);

                _logger.LogInformation("User {UserId} logged in successfully.", user.Id);

                var refreshToken = _jwtService.GenerateRefreshToken();

                user.RefreshTokenHash = BCrypt.Net.BCrypt.HashPassword(refreshToken);
                user.RefreshTokenExpiresAt = DateTime.UtcNow.AddDays(7);
                user.RefreshTokenTokenRevokedAt = null;

               await _userRepository.UpdateAsync(user);

                _logger.LogInformation("User {UserId} logged in successfully with Refresh Token.", user.Id);

                return Result<TokenResponse>.Success(new TokenResponse
                {
                    AccessToken = token,
                    RefreshToken = refreshToken
                });
            }

            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred during login for email {Email}.", dto.Email);
                return Result<TokenResponse>.Failure("Login failed. Please try again.");
            }
        }


        public async Task<Result<TokenResponse>> RefreshTokenAsync(string expiredToken, string refreshToken)
        {
            try
            {
                // 1. استخراج الإيميل من التوكن المنتهي (بدون التحقق من تاريخ الصلاحية)
                var email = _jwtService.GetEmailFromExpiredToken(expiredToken);

                if (string.IsNullOrEmpty(email))
                    return Result<TokenResponse>.Failure("Invalid token claims.");

                // 2. جلب المستخدم من القاعدة بناءً على الإيميل المستخرج
                var user = await _userRepository.GetByEmailAsync(email);
                if (user == null) return Result<TokenResponse>.Failure("User not found.");

                // 3. التحقق من الـ Refresh Token (BCrypt)
                if (!BCrypt.Net.BCrypt.Verify(refreshToken, user.RefreshTokenHash))
                {
                    _logger.LogWarning("Token mismatch for user: {email}", email);
                    return Result<TokenResponse>.Failure("Invalid session.");
                }

                // 4. التحقق من الصلاحية والانتهاء
                if (user.RefreshTokenTokenRevokedAt != null || user.RefreshTokenExpiresAt < DateTime.UtcNow)
                {
                    return Result<TokenResponse>.Failure("Session expired.");
                }

                // 5. توليد التوكنات الجديدة (الـ Rotation)
                var jwtUser = new JwtUserDto { Id = user.Id, Username = user.Username, UserType = user.UserType.ToString(), email = user.Email };
                var newAccessToken = _jwtService.GenerateToken(jwtUser);
                
                var newRefreshToken = _jwtService.GenerateRefreshToken();


                // 6. تحديث البيانات في نفس جدول المستخدم
                user.RefreshTokenHash = BCrypt.Net.BCrypt.HashPassword(newRefreshToken);
                user.RefreshTokenExpiresAt = DateTime.UtcNow.AddDays(7);
                await _userRepository.UpdateAsync(user);

                return Result<TokenResponse>.Success(new TokenResponse
                {
                    AccessToken = newAccessToken,
                    RefreshToken = newRefreshToken
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error during token refresh");
                return Result<TokenResponse>.Failure("An error occurred.");
            }
        }

        public async Task<Result<bool>> LogoutAsync(RefreshTokenDto dto)
        {
            
            try
            {
                if (dto == null)
                {
                    _logger.LogWarning("RefreshToken received null dto.");
                    return Result<bool>.Failure("Invalid user data.");
                }
                
                _logger.LogInformation(" Logout called for email={email}", dto.email);


                var user = await _userRepository.GetByEmailAsync(dto.email);


                if (user == null)
                {
                    _logger.LogWarning("Logged out successfully by email={email}", dto.email);
                    return Result<bool>.Success( true, "Logged out successfully");
                }

                var refreshValid = BCrypt.Net.BCrypt.Verify(dto.RefreshToken, user.RefreshTokenHash);

                if (!refreshValid)
                {
                    _logger.LogWarning("Logged out successfully by email={email}", dto.email);
                    return Result<bool>.Success(true, "Logged out successfully.");
                }

                user.RefreshTokenTokenRevokedAt = DateTime.UtcNow;

                await _userRepository.UpdateAsync(user);

                _logger.LogInformation("Logged out successfully by email={email}", dto.email);
                return Result<bool>.Success(true, "Logged out successfully.");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to logged out  with email={email}", dto.email);
                return Result<bool>.Failure($"Failed to logged out. : {ex.Message}");

            }
        }



    }
}
