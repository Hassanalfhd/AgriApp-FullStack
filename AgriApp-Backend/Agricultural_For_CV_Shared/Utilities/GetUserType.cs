using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Agricultural_For_CV_Shared.Enums;

namespace Agricultural_For_CV_Shared.Utilities
{
    public static class GetUserType
    {


        public static string getUserType(UserRole? userRole)
        {
            if (userRole == null)
                return "User";

            if (userRole == UserRole.Farmer)
                return "Farmer";

            if (userRole == UserRole.AgriculturalExpert)
                return "AgriculturalExpert";

            if (userRole == UserRole.Customer)
                return "Customer";


                return "Admin";


        }





    }
}
