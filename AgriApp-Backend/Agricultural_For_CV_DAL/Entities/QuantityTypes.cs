using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Agricultural_For_CV_DAL.Entities
{
    [Table("QuantityTypes")]
    public class QuantityTypes
    {

        [Key]
        public int Id { set; get; }

        [MaxLength(200)]
        [Required]
        public required string TypeName { set; get; } = string.Empty;


        [MaxLength(500)]
        public string? Description { set; get; } = string.Empty;

        public virtual ICollection<Product>?Product { set; get; }


    }
}
