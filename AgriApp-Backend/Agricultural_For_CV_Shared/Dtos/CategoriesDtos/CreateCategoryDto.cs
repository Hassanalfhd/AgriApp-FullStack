using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace Agricultural_For_CV_Shared.Dtos.CategoriesDtos
{
    public class CreateCategoryDto
    {

        public string Name { get; set; }
        public IFormFile? ImageFile { get; set; }


    }
}
