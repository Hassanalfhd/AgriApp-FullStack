using Microsoft.AspNetCore.Authorization;

namespace Agricultural_For_CV.Authorization
{
    public class FarmerOwnerOrAdminRequirement: IAuthorizationRequirement
    {
    }
}
