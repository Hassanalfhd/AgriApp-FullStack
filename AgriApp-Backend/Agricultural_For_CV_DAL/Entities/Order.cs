using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Agricultural_For_CV_Shared.Enums;


namespace Agricultural_For_CV_DAL.Entities
{
    [Table("Orders")]
    public class Order
    {

        [Key]
        public int Id{set;get;}

        [Required]
        public int CustomerId { set;get;}
        public OrderStatus Status { get; set; } = OrderStatus.Pending; // Pending, Completed, Canceled

        [ForeignKey(nameof(CustomerId))]
        public User?User{get;set;}
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public decimal TotalPrice { get; set; }


        public ICollection<OrderDetail>? OrderDetails { get; set; } = new List<OrderDetail>();  

    }
}
