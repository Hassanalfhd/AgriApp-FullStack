using Microsoft.AspNetCore.Authorization;

namespace Agricultural_For_CV.Authorization
{
    public class CustomerOwnerOrAdminRequirement: IAuthorizationRequirement
    {
    }
}
