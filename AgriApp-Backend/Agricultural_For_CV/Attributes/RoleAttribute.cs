using Agricultural_For_CV_Shared.Enums;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System.Security.Claims;


[AttributeUsage(AttributeTargets.Method | AttributeTargets.Class, AllowMultiple = true)]
public class RoleAttribute : Attribute, IAsyncAuthorizationFilter
{
    private readonly UserRole[] _allowedRoles;

    public RoleAttribute(params UserRole[] allowedRoles)
    {
        _allowedRoles = allowedRoles;
    }

    public async Task OnAuthorizationAsync(AuthorizationFilterContext context)
    {
        // 1. السماح بالوصول المجهول [AllowAnonymous]
        if (context.ActionDescriptor.EndpointMetadata.Any(em => em is AllowAnonymousAttribute))
            return;

        var user = context.HttpContext.User;

        // 2. التحقق من المصادقة عبر نظام الهوية الرسمي
        // هذا يحل مشكلة كسر Identity.IsAuthenticated
        if (user.Identity == null || !user.Identity.IsAuthenticated)
        {
            context.Result = new UnauthorizedObjectResult(new { message = "Unauthorized: Please login first" });
            return;
        }

        // 3. جلب الـ Role من الـ Claims
        // نقرأ القيمة المخزنة في التوكن (التي وضعها الـ Middleware)
        var roleClaim = user.FindFirst(ClaimTypes.Role)?.Value;

        // تحويل الـ Enums المسموح بها إلى نصوص (أرقام) للمقارنة
        var allowedRolesAsStrings = _allowedRoles.Select(r => r.ToString()).ToList();

        if (string.IsNullOrEmpty(roleClaim) || !allowedRolesAsStrings.Contains(roleClaim))
        {
            // 4. نقطة الـ Audit الاحترافية
            // هنا يمكنك استدعاء خدمة الـ Audit لتسجيل محاولة الدخول المرفوضة
            await LogForbiddenAccess(context, user);

            context.Result = new JsonResult(new { message = "Forbidden: Access Denied" })
            {
                StatusCode = StatusCodes.Status403Forbidden
            };
        }
    }

    private async Task LogForbiddenAccess(AuthorizationFilterContext context, ClaimsPrincipal user)
    {
        // استخدام الـ Logger أو الـ AuditService المسجل في الـ DI
        var logger =  context.HttpContext.RequestServices.GetService<ILogger<RoleAttribute>>();
        var userId = user.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        
          logger?.LogWarning("Security Alert: User {UserId} tried to access {Action} without required roles.",
            userId, context.ActionDescriptor.DisplayName);
    }
}