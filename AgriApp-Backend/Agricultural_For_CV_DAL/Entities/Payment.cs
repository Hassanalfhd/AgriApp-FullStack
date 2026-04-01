using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Agricultural_For_CV_DAL.Entities
{
    [Table("Payments")]
    public class Payment
    {

        [Key]
        public int Id { get; set; }

        [Required]
        public int OrderId { get; set; }

        [ForeignKey(nameof(OrderId))]
        public Order Order { get; set; }

        [Required]
        public decimal Amount { get; set; }
        [Required]
        public string PaymentMethod { get; set; } = "StripeTest"; // يمكن تغييرها لاحقاً
        [Required]
        public string Status { get; set; } = "Pending"; // Pending / Paid / Failed
        public string? TransactionId { get; set; }
        public DateTime? PaymentDate { get; set; }
        [Required]
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    }
}
