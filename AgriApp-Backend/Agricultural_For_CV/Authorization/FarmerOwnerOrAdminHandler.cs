using System.Security.Claims;
using Agricultural_For_CV_Shared.Enums;
using Microsoft.AspNetCore.Authorization;

namespace Agricultural_For_CV.Authorization
{
    public class FarmerOwnerOrAdminHandler: AuthorizationHandler<FarmerOwnerOrAdminRequirement, int>
    {

        protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, FarmerOwnerOrAdminRequirement requirement, int farmerId)
        {

            if (context.User.IsInRole(UserRole.Admin.ToString()))
            {
                context.Succeed(requirement);
                return Task.CompletedTask;
            }


            var userId = context.User.FindFirstValue(ClaimTypes.NameIdentifier);

            if (int.TryParse(userId, out int authenticatedStudentId) && authenticatedStudentId == farmerId  )
            {
                context.Succeed(requirement);
            } 
            


            return Task.CompletedTask;

        }
   
    
    }
}
