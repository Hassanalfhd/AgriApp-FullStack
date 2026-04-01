using Agricultural_For_CV_Shared.Enums;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;

namespace Agricultural_For_CV.Authorization
{
    public class CustomerOwnerOrAdminHandler : AuthorizationHandler<CustomerOwnerOrAdminRequirement, int>
    {
        protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, CustomerOwnerOrAdminRequirement requirement, int customerId)
        {

            if (context.User.IsInRole(UserRole.Admin.ToString()))
            {
                context.Succeed(requirement);
                return Task.CompletedTask;
            }


            var userId = context.User.FindFirstValue(ClaimTypes.NameIdentifier);

            if (int.TryParse(userId, out int authenticatedStudentId) && authenticatedStudentId == customerId )
            {
                context.Succeed(requirement);
            }

            return Task.CompletedTask;

        }


    }
}
