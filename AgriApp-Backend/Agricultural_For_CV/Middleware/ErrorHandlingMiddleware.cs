using System.Net;
using System.Text.Json;
using Agricultural_For_CV_Shared.Results;
using Microsoft.Extensions.Logging;

namespace Agricultural_For_CV.Middleware
{
    public class ErrorHandlingMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<ErrorHandlingMiddleware> _logger;

        public ErrorHandlingMiddleware(RequestDelegate next, ILogger<ErrorHandlingMiddleware> logger)
        {
            _next = next;
            _logger = logger;
        }
        
        public async Task Invoke(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (Exception ex)
            {
                // سجل الاستثناء
                _logger.LogError(ex, "Unhandled exception occurred while processing request.");

                // we will remove it in production because its Synchronous and cause blocking
                Console.WriteLine("-----------********___________________");
                Console.WriteLine(ex + "Unhandled exception occurred while processing request.");
                Console.WriteLine(ex.Message + "Unhandled exception occurred while processing request.");
                Console.WriteLine("-----------********___________________");

                // إعداد الرد
                context.Response.ContentType = "application/json";
                context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;

                var response = Result.Failure("An unexpected error occurred."); // نص عام للمستخدم
                var json = JsonSerializer.Serialize(response);

                await context.Response.WriteAsync(json);
            }
        }
    }
}
