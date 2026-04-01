public class ProfilingMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<ProfilingMiddleware> _logger;

    public ProfilingMiddleware(RequestDelegate next, ILogger<ProfilingMiddleware> logger)
    {
        _next = next;
        _logger = logger;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        // --- مرحلة الدخول (Request Path) ---
        var stopwatch = System.Diagnostics.Stopwatch.StartNew();

        _logger.LogInformation("--- [Start] Request: {Method} {Path} ---",
            context.Request.Method, context.Request.Path);

        // تمرير "الحقيبة" (HttpContext) للـ Middleware التالي في السلسلة
        await _next(context);

        // --- مرحلة العودة (Response Path) ---
        // الكود هنا لا ينفذ إلا بعد أن يذهب الطلب للـ Controller ويعود بالنتيجة
        stopwatch.Stop();

        var elapsedMs = stopwatch.ElapsedMilliseconds;
        _logger.LogInformation("--- [End] Request processed in {ElapsedMs}ms with Status: {Status} ---",
            elapsedMs, context.Response.StatusCode);



    }
}