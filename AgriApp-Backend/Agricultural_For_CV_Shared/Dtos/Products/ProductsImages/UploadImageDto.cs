using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace Agricultural_For_CV_Shared.Dtos.ProductsImages
{
    public class UploadImageDto
    {
        public IFormFile? File { get; set; }

        public int ImageOrder { get; set; }
    }
}
