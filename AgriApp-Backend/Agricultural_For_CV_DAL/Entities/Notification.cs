using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Agricultural_For_CV_DAL.Entities
{

    [Table("Notifications")]
    public class Notification
    {

        [Key]
        public int Id { get; set; }

        [Required]
        public int UserId { get; set; }                  // المستخدم المستلم
        [Required]
        public string Message { get; set; } = string.Empty;
        [Required]
        public bool IsRead { get; set; } = false;       // حالة القراءة
        [Required]
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;


        // دعم الربط بالمنتجات والطلبات
        public int? ProductId { get; set; }
        public int? OrderId { get; set; }

        // علاقات اختيارية
        public Product? Product { get; set; }
        public Order? Order { get; set; }
    }
}
