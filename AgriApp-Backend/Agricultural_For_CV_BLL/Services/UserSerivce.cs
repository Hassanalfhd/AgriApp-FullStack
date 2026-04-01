using Agricultural_For_CV_DAL.Interfaces;
using Agricultural_For_CV_Shared.Dtos.UserDtos;
using Agricultural_For_CV_Shared.Interfaces;
using Agricultural_For_CV_Shared.Results;
using Agricultural_For_CV_Shared.Settings;
using Microsoft.Extensions.Options;
using Microsoft.Extensions.Logging;
using Agricultural_For_CV_Shared.Enums;
using Agricultural_For_CV_Shared.Dtos.Products;
using Agricultural_For_CV_DAL.Entities;

namespace Agricultural_For_CV_BLL.Services
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;
        private readonly IImageService _imageService;
        private readonly AppSettings _settings;
        private readonly ILogger<UserService> _logger;

        public UserService(
            IUserRepository userRepository,
            IImageService imageService,
            IOptions<AppSettings> setting,
            ILogger<UserService> logger)
        {
            _userRepository = userRepository ?? throw new ArgumentNullException(nameof(userRepository));
            _imageService = imageService ?? throw new ArgumentNullException(nameof(imageService));
            _settings = setting?.Value ?? throw new ArgumentNullException(nameof(setting));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }


        #region Get All & By Id & GetPagedUsersAsync

        public async Task<Result<IEnumerable<UserResponseDto>>> GetAllAsync()
        {

            try
            {
         
                _logger.LogInformation("GetAllAsync called.");
                var users = await _userRepository.GetAllAsync();

                if(users == null)
                {

                    _logger.LogError("Retrieves Null of Data.");
                    return Result<IEnumerable<UserResponseDto>>.Failure("Retrieves Null of Data.");
                }
                var dtos = users.Select(u => new UserResponseDto
                {
                    Id = u.Id,
                    Username = u.Username ?? "",
                    Email = u.Email ?? "",
                    UserType = ((UserRole)u.UserType).ToString(),
                    fullName = u.fullName ?? "",
                    ImageFile = u.ImageFile ?? "",
                    CreatedAt = u.CreatedAt 
                });

                _logger.LogInformation("GetAllAsync succeeded. Count={Count}", dtos.Count());
                return Result<IEnumerable<UserResponseDto>>.Success(dtos);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "GetAllAsync failed.");
                return Result<IEnumerable<UserResponseDto>>.Failure("Failed to retrieve users.");
            }
        }

        public async Task<Result<PagedResult<UserResponseDto>>> GetPagedUsersAsync(int page, int pageSize)
        {

            try
            {
                _logger.LogInformation("GetAllAsync called.");
                var (items, totalCount) = await _userRepository.GetPagedUsersAsync(page, pageSize);
                var dtos = items.Select(u => new UserResponseDto
                {
                    Id = u.Id,
                    Username = u.Username,
                    Email = u.Email,
                    UserType = ((UserRole)u.UserType).ToString(),
                    Phone = u.Phone,
                    IsActive = u.isActive,
                    fullName = u.fullName,
                    ImageFile = u.ImageFile,
                    CreatedAt = u.CreatedAt

                }).ToList();


                _logger.LogInformation("GetAllAsync succeeded. Count={Count}", dtos.Count());
                return Result<PagedResult<UserResponseDto>>.Success(new PagedResult<UserResponseDto>
                {
                    Items = dtos,
                    TotalCount = totalCount,
                    Page = page,
                    PageSize= pageSize
                    
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "GetAllAsync failed.");
                return Result<PagedResult<UserResponseDto>>.Failure("Failed to retrieve users.");
            }
        }

        public async Task<Result<UserResponseDto>> GetByIdAsync(int id)
        {
            _logger.LogInformation("GetByIdAsync called for id={Id}", id);

            if (id <= 0)
            {
                _logger.LogWarning("GetByIdAsync received invalid id={Id}", id);
                return Result<UserResponseDto>.Failure("Invalid user id.");
            }

            try
            {
                var user = await _userRepository.GetByIdAsync(id);
                if (user == null)
                {
                    _logger.LogWarning("User not found with id={Id}", id);
                    return Result<UserResponseDto>.Failure("User not found.");
                }



                var dto = new UserResponseDto
                {
                    Id = user.Id,
                    Username = user.Username,
                    fullName = user.fullName,
                    Email = user.Email,
                    UserType = ((UserRole)user.UserType).ToString(),
                    ImageFile = user.ImageFile,
                    CreatedAt = user.CreatedAt,
                    Phone = user.Phone,
                    IsActive = user.isActive
                    
                };


                _logger.LogInformation("GetByIdAsync succeeded for id={Id}", id);
                return Result<UserResponseDto>.Success(dto);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "GetByIdAsync failed for id={Id}", id);
                return Result<UserResponseDto>.Failure("Failed to retrieve user.");
            }
        }

        #endregion

        #region  Exists User By Email  
        public async Task<Result<bool>> ExistsByEmailAsync(string email)
        {
            try
            {

                _logger.LogInformation("GetAllAsync called.");
                 var user = await _userRepository.ExistsByEmailAsync(email);

                if (!user)
                {
                    _logger.LogError("User not found.");
                    return Result<bool>.Failure($"User not found with email {email}.");
                }
                
                _logger.LogInformation("Users is Exists By Email succeeded. email={email}", email);
                return Result<bool>.Success(user);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Failed to found  User with email {email}.");
                return Result<bool>.Failure($"Failed to found  User with email {email}.");
            }


        }

        #endregion

        #region Delete User

        public async Task<Result<bool>> DeleteAsync(int id)
        {
            _logger.LogInformation("DeleteAsync called for id={Id}", id);

            if (id <= 0)
            {
                _logger.LogWarning("DeleteAsync received invalid id={Id}", id);
                return Result<bool>.Failure("Invalid user id.");
            }

            try
            {
                var user = await _userRepository.GetByIdAsync(id);
                if (user == null)
                {
                    _logger.LogWarning("DeleteAsync: user not found for id={Id}", id);
                    return Result<bool>.Failure("User not found.");
                }

                // Delete DB record
                await _userRepository.DeleteAsync(id);

                // Delete image file if exists
                if (!string.IsNullOrEmpty(user.ImageFile))
                {
                    _imageService.DeleteImage(user.ImageFile);
                }

                _logger.LogInformation("DeleteAsync succeeded for id={Id}", id);
                return Result<bool>.Success(true, "User deleted successfully.");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "DeleteAsync failed for id={Id}", id);
                return Result<bool>.Failure("Failed to delete user.");
            }
        }

        #endregion

        #region Update User & Update User Type

    
        public async Task<Result<bool>> SetUserAccountStatusAsync(int userId, bool isActive)
        {
            if (userId <= 0)
            {
                _logger.LogWarning("AccountStatus received Invalid user Id.");
                return Result<bool>.Failure("Invalid user Id.");
            }


            _logger.LogInformation("UpdateAsync called for id={Id}", userId);


            try
            {
                var user = await _userRepository.GetByIdAsync(userId);
                if (user == null)
                {
                    _logger.LogWarning("UpdateAsync: user not found for id={Id}", userId);
                    return Result<bool>.Failure("User not found.");
                }


                // Update scalar fields
                user.isActive = isActive;

                await _userRepository.UpdateAsync(user);


                _logger.LogInformation("UpdateAsync succeeded for id={Id}", userId);
                return Result<bool>.Success(true, "User Status updated successfully.");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "UpdateAsync failed for id={Id}", userId);
                return Result<bool>.Failure($"Failed to update user: {ex.Message}");
            }


        }
        public async Task<Result<UserResponseDto>> UpdateAsync(UserUpdateDto dto)
        {
            if (dto == null)
            {
                _logger.LogWarning("UpdateAsync received null dto.");
                return Result<UserResponseDto>.Failure("Invalid user data.");
            }



            try
            {

                _logger.LogInformation("UpdateAsync called for id={Id}", dto.Id);
    
                var user = await _userRepository.GetByIdAsync(dto.Id);
                if (user == null)
                {
                    _logger.LogWarning("UpdateAsync: user not found for id={Id}", dto.Id);
                    return Result<UserResponseDto>.Failure("User not found.");
                }


                // Update scalar fields
                user.Username = string.IsNullOrEmpty(dto.Username) ? user.Username : dto.Username.Trim() ;
                user.Email = string.IsNullOrEmpty(dto.Email)? user.Email : dto.Email.Trim() ;
                user.Phone = string.IsNullOrEmpty(dto.Phone)? user.Phone: dto.Phone.Trim() ;
                user.fullName = string.IsNullOrEmpty(dto.FullName) ? user.fullName : dto.FullName.Trim() ;
                

                // Handle image update
                if (dto.ImageFile != null)
                {
                    var result = await _imageService.SaveImageAsync(dto.ImageFile, _settings.ImagePaths.Users);
                    if (!result.IsSuccess)
                    {
                        _logger.LogWarning("Failed to save user image: {Error}", result.Message);
                    }
                    else
                    {
                        // Optionally delete old image
                        if (!string.IsNullOrEmpty(user.ImageFile))
                            _imageService.DeleteImage(user.ImageFile);

                        user.ImageFile = result.Data!;
                    }
                }

                await _userRepository.UpdateAsync(user);

                var updatedDto = new UserResponseDto
                {
                    Id = user.Id,
                    Username = user.Username,
                    fullName = user.fullName,
                    Email = user.Email,
                    UserType = ((UserRole)user.UserType).ToString(),
                    ImageFile = user.ImageFile,
                    CreatedAt = user.CreatedAt,
                    Phone = user.Phone,
                    IsActive = user.isActive
                };

                _logger.LogInformation("UpdateAsync succeeded for id={Id}", dto.Id);
                return Result<UserResponseDto>.Success(updatedDto, "User updated successfully.");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "UpdateAsync failed for id={Id}", dto.Id);
                return Result<UserResponseDto>.Failure($"Failed to update user: {ex.Message}");
            }
        }

        #endregion
    }
}
