using Agricultural_For_CV_Shared.Dtos.UserDtos;
using Agricultural_For_CV_Shared.Interfaces;
using Agricultural_For_CV_Shared.Results;
using Agricultural_For_CV_Shared.Settings;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Microsoft.AspNetCore.RateLimiting;
namespace Agricultural_For_CV.Controllers.v1
{
    
    [ApiController]
    [Route("api/v{version:apiVersion}/auth")]
    [ApiVersion("1.0")]
    [EnableRateLimiting("AuthLimiter")]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;
        private readonly AppSettings _appSettings;
        private readonly IJwtService _jwtService;
        public AuthController(IAuthService authService, IOptions<AppSettings> appSettings, IJwtService jwtService )
        {
            _authService = authService;
            _appSettings = appSettings.Value;
            _jwtService = jwtService;
        }


        /// <summary>
        /// Authenticates a user and returns a JWT token if successful.
        /// </summary>
        /// <param name="dto">The login credentials (username/email and password).</param>
        /// <returns>JWT token string if login succeeds.</returns>
        /// <response code="200">Login successful, returns JWT token.</response>
        /// <response code="401">Unauthorized, credentials are invalid.</response>
        [AllowAnonymous]
        [HttpPost("login")]
        [ProducesResponseType(typeof(Result<string>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<IActionResult> Login([FromBody]LoginDto dto)
        {
            var result = await _authService.LoginAsync(dto);

            if (!result.IsSuccess)
                return Unauthorized(result);

            SetRefreshTokenCookie(result.Data!.RefreshToken);


            return Ok(new
            {
                token = result.Data!.AccessToken,
                error = result.Error,
                isSuccess = result.IsSuccess,
                message = result.Message
            });
        }

        private void SetRefreshTokenCookie(string refreshToken)
        {
            var cookieOptions = new CookieOptions
            {
                HttpOnly = true,      
                Secure = true,          
                SameSite = SameSiteMode.None,
                Expires = DateTime.UtcNow.AddDays(7)
            };
            Response.Cookies.Append("refreshToken", refreshToken, cookieOptions);
        }

        [ProducesResponseType(typeof(Result<string>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [HttpPost("refresh-token")]
        public async Task<IActionResult> RefreshToken()
        {

            var refreshToken = Request.Cookies["refreshToken"];

            var expiredAccessToken = Request.Headers["Authorization"].ToString().Replace("Bearer ", "");

            if (string.IsNullOrEmpty(refreshToken) || string.IsNullOrEmpty(expiredAccessToken))
                return Unauthorized("Missing tokens.");

            
            var result = await _authService.RefreshTokenAsync(refreshToken, expiredAccessToken );

            if(!result.IsSuccess) return Unauthorized(result);

            SetRefreshTokenCookie(result.Data!.RefreshToken);

            return Ok(new
            {
                token = result.Data!.AccessToken,
                error = result.Error,
                isSuccess = result.IsSuccess,
                message = result.Message
            });

        }


        [AllowAnonymous]
        [ProducesResponseType(typeof(Result<bool>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [HttpPost("logout")]
        public async Task<IActionResult> Logout(RefreshTokenDto dto)
        {

            var result = await _authService.LogoutAsync(dto);

            if (!result.IsSuccess)
            {
                // error that service have issue whene remove  the token or logged out
            }


            return Ok(result);

        }

        /// <summary>
        /// Registers a new user.
        /// </summary>
        /// <param name="dto">The user data for registration.</param>
        /// <returns>The registered user data if successful.</returns>
        /// <response code="201">User registered successfully.</response>
        /// <response code="400">Returned when registration fails (e.g., username/email already exists).</response>

        [AllowAnonymous]
        [HttpPost("register")]
        [ProducesResponseType(typeof(Result<UserResponseDto>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Register([FromBody] UserDto dto)
        {
            var result = await _authService.RegisterAsync(dto);

            if (!result.IsSuccess)
            {
                return BadRequest(result);
            }

            return CreatedAtAction(nameof(Register), new { id = result.Data!.Id }, result.Data);
        }




    }
}
