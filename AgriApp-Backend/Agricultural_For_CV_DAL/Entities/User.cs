using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Agricultural_For_CV_DAL.Entities
{
    [Table("Users")]
    public class User

    {
        [Key]
        public int Id { get; set; }

        [Required]
        [MaxLength(200)]
        public string fullName { get; set; } = string.Empty;

        [Required]
        [MaxLength(100)]
        public string Username { get; set; } = string.Empty;

        [Required]
        [EmailAddress]
        public string Email { get; set; } = string.Empty;

        [Required]
        [MaxLength(50)]
        public string PasswordHash { get; set; } = string.Empty;


        [Required]
        [Range(1,3)]
        public int UserType { get; set; } // 1 = Admin, 2 = Farmer, 3 = Customer

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        [MaxLength(500)]
        public string? ImageFile { get; set; } = string.Empty;

        
        [MaxLength(14)]
        public string? Phone { get; set; }

        [Required]
        public bool isActive { get; set; } = true;


        [MaxLength(200)]
        public string? RefreshTokenHash { get; set; } 

        public DateTime? RefreshTokenExpiresAt { get; set; }
        public DateTime? RefreshTokenTokenRevokedAt { get; set; }




        //relation  with its crops
        public virtual ICollection<Crop>? Crops { get; set; }

        //relation  with its Product
        public virtual ICollection<Product>?Product { set; get; }
        //relation  with its Orders
        public ICollection<Order>? Orders { get; set; }
        public ICollection<OrderDetail>? OrderDetails { get; set; }


    }

}
