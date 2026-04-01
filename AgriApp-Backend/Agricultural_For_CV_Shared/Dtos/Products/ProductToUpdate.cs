using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace Agricultural_For_CV_Shared.Dtos.Products
{

    public class ProductToUpdate
    {

        public int Id { get; set; }

        public string Name { get; set; } = string.Empty;

        public string? Description { get; set; }

        public int QuantityInStock { get; set; }

        public int CropTypeId { get; set; }

        public int QuantityTypeId { get; set; }

        public decimal Price { get; set; }


        public int CreatedBy { get; set; }

    }
}
