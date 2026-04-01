using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace Agricultural_For_CV_Shared.Dtos.Products
{
    public class ProductToCreate
    {


        [Required]
        public string Name { get; set; } = string.Empty; 

        public string? Description { get; set; } 

        [Required]
        public int QuantityInStock { get; set; }

        [Required]
        public int CropTypeId { get; set; }

        [Required]
        public int QuantityTypeId { get; set; }

        [Required]
        public decimal Price { get; set; } = decimal.Zero;

        [Required]
        public int CreatedBy { get; set; }

    }
}
