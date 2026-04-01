using Agricultural_For_CV_Shared.Interfaces;
using System.Security.Claims;


namespace Agricultural_For_CV.Middleware
{
    public class JwtMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<JwtMiddleware> _logger;

        public JwtMiddleware(RequestDelegate next, ILogger<JwtMiddleware> logger)
        {
            _next = next;
            _logger = logger;
        }

        public async Task Invoke(HttpContext context, IJwtService jwtService)
        {
            var authHeader = context.Request.Headers["Authorization"].FirstOrDefault();
            var token = authHeader?.StartsWith("Bearer ") == true ? authHeader[7..] : authHeader;

            if (!string.IsNullOrEmpty(token))
            {
                try
                {
                    var principal = jwtService.ValidateToken(token);
                    if (principal?.Identity?.IsAuthenticated == true)
                    {
                        context.User = principal;
                        // Audit: تسجيل دخول ناجح
                        var userId = principal.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                        _logger.LogInformation("User {UserId} authenticated successfully at {Time}", userId, DateTime.UtcNow);
                    }
                }
                catch (Exception ex)
                {
                    // Audit: تسجيل محاولة دخول فاشلة أو توكن مزور
                    _logger.LogWarning("Invalid token attempt: {Message}", ex.Message);
                }
            }

            await _next(context);
        }
    }
}
