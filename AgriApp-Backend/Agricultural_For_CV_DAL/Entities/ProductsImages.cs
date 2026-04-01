using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Agricultural_For_CV_DAL.Entities
{

    [Table("ProductsImages")]
    public class ProductsImages
    {
        [Key]
        public int Id { get; set; }

    
        [MaxLength(500)]
        public string? ImagePath { get; set; } = string.Empty;
        
        [Required]
        public int ProductId { get; set; }

        [Required]
        public int ImageOrder { get; set; }

        [ForeignKey("ProductId")]
        public Product? Product { get; set; } = null;


    }
}
