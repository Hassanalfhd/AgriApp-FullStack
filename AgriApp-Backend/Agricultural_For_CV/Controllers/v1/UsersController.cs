using System.Security.Claims;
using Agricultural_For_CV.Helpers;
using Agricultural_For_CV_Shared.Dtos.UserDtos;
using Agricultural_For_CV_Shared.Enums;
using Agricultural_For_CV_Shared.Interfaces;
using Agricultural_For_CV_Shared.Results;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Agricultural_For_CV.Controllers.v1
{
    [Route("api/v{version:apiVersion}/users")]
    [ApiController]
    [ApiVersion("1.0")]
    [Authorize]
    public class UsersController : ControllerBase
    {

        private readonly IUserService _userService;
        public UsersController(IUserService userService)
        {
            _userService = userService;

        }




        /// <summary>
        /// Retrieves all users in the system.
        /// </summary>
        /// <remarks>
        /// Requires authorization. Optionally, can restrict access to Admin roles using [Role("Admin")].
        /// Each user's image URL is converted to a full URL.
        /// </remarks>
        /// <returns>A list of users.</returns>
        /// <response code="200">Returns the list of users.</response>
        /// <response code="400">Returned when an error occurs while fetching users.</response>

        [HttpGet]
        [ProducesResponseType(typeof(Result<IEnumerable<UserResponseDto>>),StatusCodes.Status200OK)]
        [AllowAnonymous]
        //[Role(UserRole.Admin)]
        public async Task<IActionResult> GetAllUsers()
        {
            var result = await _userService.GetAllAsync();

            
            if (result == null)
            {
                return NoContent();
            }


            string baseUrl = "";
            if (Request != null)
                baseUrl = $"{Request.Scheme}://{Request.Host.Value}/";


            return Ok(result.Data?.Select(u => new
            {
                u.Id,
                u.fullName,
                u.Username,
                u.Email,
                u.Phone,
                u.UserType ,
                u.CreatedAt,
                u.IsActive,
                ImageFile = string.IsNullOrEmpty(u.ImageFile) ? null : UrlHelper.GetFullUrl(baseUrl, u.ImageFile)
            }));


        }



        [HttpGet("UsersPage")]
        [AllowAnonymous]
        //[Role(["Admin"])]
        //[ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore =true)]
        public async Task<IActionResult> GetPagedUsers([FromQuery]int page =1, [FromQuery]int pageSize = 10)
        {
            var result = await _userService.GetPagedUsersAsync(page, pageSize);

            
            if (result == null || result.Data == null)
            {
                return NotFound("There is no data.");
            }

            string baseUrl = "";
            if (Request != null)
                baseUrl = $"{Request.Scheme}://{Request.Host.Value}/";

            result.Data!.Items.ForEach(item =>
            {
                item.ImageFile = UrlHelper.GetFullUrl(baseUrl, item.ImageFile);
            });


            return Ok(result.Data);

        }


        /// <summary>
        /// Retrieves a user by their unique identifier.
        /// </summary>
        /// <param name="userId">The unique identifier of the user.</param>
        /// <returns>The user data including the full image URL.</returns>
        /// <response code="200">Returns the user data successfully.</response>
        /// <response code="400">Returned when the user does not exist or another error occurs.</response>

        [HttpGet("userId")]
        [ProducesResponseType(typeof(Result<UserResponseDto>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> GetUserById(int userId, [FromServices] IAuthorizationService authorizationService)
        {

            var authResult = await authorizationService.AuthorizeAsync(User, userId, "FarmerOwnerOrAdmin");


            if (!authResult.Succeeded)
                return Forbid(); // Returns HTTP 403 Forbidden

            var result = await _userService.GetByIdAsync(userId);
            string baseUrl = "";
            if (Request != null)
            baseUrl = $"{Request.Scheme}://{Request.Host.Value}/";
          

            if (!result.IsSuccess)
                return BadRequest(result);

            if (result.Data?.ImageFile != null)
                result.Data.ImageFile = string.IsNullOrEmpty(result.Data.ImageFile) ? null : UrlHelper.GetFullUrl(baseUrl, result.Data.ImageFile);


            return Ok(result);
        }


        [HttpGet("profile")]
        [ProducesResponseType(typeof(Result<UserResponseDto>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> GetUserProfile()
        {
            // 🧩 استخراج userId من التوكن
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            if (string.IsNullOrEmpty(userIdClaim) || !int.TryParse(userIdClaim, out int userId))
                return BadRequest(Result<UserResponseDto>.Failure("Invalid or missing user ID in token."));

            // 🔍 جلب المستخدم من الخدمة
            var result = await _userService.GetByIdAsync(userId);
            string baseUrl = $"{Request.Scheme}://{Request.Host.Value}/";

            if (!result.IsSuccess)
                return BadRequest(result);

            // 🖼️ تعديل رابط الصورة ليصبح رابط كامل
            if (!string.IsNullOrEmpty(result.Data?.ImageFile))
                result.Data.ImageFile = UrlHelper.GetFullUrl(baseUrl, result.Data.ImageFile);

            return Ok(result.Data);
        }



        [HttpPut("profile/me")]
        [ProducesResponseType(typeof(Result<UserResponseDto>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> UpdateUserProfile(UserUpdateDto dto, [FromServices]IAuthorizationService authorizationService)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(dto);
            }

            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            if (string.IsNullOrEmpty(userIdClaim) || !int.TryParse(userIdClaim, out int userId))
                return BadRequest(Result<UserResponseDto>.Failure("Invalid or missing user ID in token."));

            dto.Id = userId;

            var authResult = await authorizationService.AuthorizeAsync(User, dto.Id, "FarmerOwnerOrAdmin");


            if (!authResult.Succeeded)
                return Forbid(); // Returns HTTP 403 Forbidden
            

            var result = await _userService.UpdateAsync(dto);

            if (!result.IsSuccess)
                return BadRequest(result);

            
            return Ok(result.Data);
        }


        /// <summary>
        /// Updates an existing user's information.
        /// </summary>
        /// <param name="dto">The updated user data.</param>
        /// <returns>The updated user information.</returns>
        /// <response code="200">User updated successfully.</response>
        /// <response code="400">Returned when the update fails due to invalid data or other issues.</response>

        [HttpPut("Update")]
        [ProducesResponseType(typeof(Result<UserResponseDto>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [Role(UserRole.Admin)]
        public async Task<IActionResult>UpdateUser(UserUpdateDto dto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(dto);
            }


            var result = await _userService.UpdateAsync(dto);



            if (!result.IsSuccess)
                return BadRequest(result);

            return Ok(result.Data);
        }


        /// <summary>
        /// Deletes a user by their unique identifier.
        /// </summary>
        /// <param name="userId">The ID of the user to delete.</param>
        /// <remarks>
        /// Only accessible by Admin role.
        /// </remarks>
        /// <returns>Boolean indicating success of deletion.</returns>
        /// <response code="200">User deleted successfully.</response>
        /// <response code="400">Returned when deletion fails due to invalid request.</response>
        /// <response code="404">Returned when the user does not exist.</response>
        [HttpDelete("Delete/{userId:int}")]
        [ProducesResponseType(typeof(Result<bool>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [Role(UserRole.Admin)]
        public async Task<IActionResult>DeleteUser(int userId)
        {
            var result = await _userService.DeleteAsync(userId);

            if (!result.IsSuccess)
                return BadRequest(result);

            return Ok(result);

        }



        /// <summary>
        /// Sets the status of a user account (activate or deactivate).
        /// </summary>
        /// <remarks>
        /// Only an Admin can perform this action. 
        /// Use <paramref name="isActive"/> as true to activate the account, false to deactivate.
        /// </remarks>
        /// <param name="userId">The ID of the user whose account status will be updated.</param>
        /// <param name="isActive">The desired account status (true = active, false = inactive).</param>
        /// <returns>
        /// Returns <see cref="Result{bool}"/> indicating success or failure of the operation.
        /// HTTP 200 OK if successful, 400 Bad Request if the operation fails, 404 Not Found if the user does not exist.
        /// </returns>
        [HttpPatch("{userId}/status")]
        [ProducesResponseType(typeof(Result<bool>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [Role(UserRole.Admin)]
        public async Task<IActionResult> SetUserAccountStatus([FromRoute] int userId, [FromQuery] bool isActive)
        {
            var result = await _userService.SetUserAccountStatusAsync(userId, isActive);

            if (!result.IsSuccess)
                return BadRequest(result);

            return Ok(result);
        }





    }
}
