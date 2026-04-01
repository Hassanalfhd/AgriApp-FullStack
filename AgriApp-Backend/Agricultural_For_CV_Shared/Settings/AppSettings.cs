using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Agricultural_For_CV_Shared.Settings
{
    public class AppSettings
    {
        public string SecretKey { get; set; } = string.Empty;
        public string Issuer { get; set; } = string.Empty;
        public string Audience { get; set; } = string.Empty;
        public int JwtExpiryMinutes { get; set; } = 60;
        public string BaseUrl { get; set; } = string.Empty;
        public string wwwrootPath { get; set; } = string.Empty;
        public ImagePaths ImagePaths { get; set; } = new ImagePaths();
    }

    public class ImagePaths
    {
        public string Users { get; set; } = "images/users";
        public string Crops { get; set; } = "images/Crops";
        public string Categories { get; set; } = "images/categories";
        public string Products { get; set; } = "images/Products";
    }


}
