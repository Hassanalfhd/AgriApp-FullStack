using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace Agricultural_For_CV_Shared.Dtos.ProductsImages
{
    public class ProductImageToAddDto
    {
        public IFormFile ImageFile { get; set; } = default!;
        public int ImageOrder { get; set; } = 0;
    }

    public class ProductImageResponseDto
    {
        public int Id { get; set; }
        public string ImagePath { get; set; } = string.Empty;
        public int ImageOrder { get; set; }
    }
}
