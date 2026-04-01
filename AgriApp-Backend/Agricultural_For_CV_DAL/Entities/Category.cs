using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Agricultural_For_CV_DAL.Entities
{
    [Table("Categories")]
  public class Category
    {

        [Key]
        public int Id { get; set; }


        [Required]
        [MaxLength(100)]
        public string Name { get; set; }

        public string? ImagePath { get; set; }

        // Navigation Property: كل المحاصيل تحت هذه الفئة
        public virtual ICollection<Crop>? Crops { get; set; }
    }


}
