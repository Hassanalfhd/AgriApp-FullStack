using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace Agricultural_For_CV_Shared.Dtos.Products
{
    public  class ProductsResponseDto
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string? Description { get; set; } 
        public string? QuantityName { set; get; }
        public int CropTypeId { get; set; }
        public string?CropName { get; set; }
        public decimal Price { get; set; } = decimal.Zero;
        public int? CreatedBy { get; set; }
        public string ?CreatedByName { get; set; }
        public string ? CreatedByImage { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public string? Image { get; set; }

        public int? QuantityInStock { set; get; }
        public int? QuantityTypeId { set; get; }




    }



    public class ProductResponseDto
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string? Description { get; set; }
        public decimal Price { get; set; } = decimal.Zero;
        public int CreatedBy { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public string? QuantityName { get; set; }
        public string? CreatedByName { get; set; }
        public int ? CropTypeId { get; set; }
        public string? CropName { get; set; }
        public List<string>? Images{ get; set; }
        public string? CreatedByImage { get; set; }

        public int QuantityInStock { set; get; }
        public int QuantityTypeId { set; get; }


    }

}
