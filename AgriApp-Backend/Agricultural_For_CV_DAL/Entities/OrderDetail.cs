using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using Agricultural_For_CV_Shared.Enums;

namespace Agricultural_For_CV_DAL.Entities
{

    [Table("OrderDetails")]
    public class OrderDetail
    {
        public int Id { get; set; }

        public int OrderId { get; set; }
        [ForeignKey(nameof(OrderId))]

        public Order? Order { get; set; }
        public int ProductId { get; set; }

        [ForeignKey(nameof(ProductId))]
        public Product? Product { get; set; }

        public int FarmerId { get; set; }

        [ForeignKey(nameof(FarmerId))]
        public User? User{ get; set; }
        public int Quantity { get; set; }
        public decimal UnitPrice { get; set; }
        public ItemStatus Status { get; set; } = ItemStatus.Pending; // Pending, Completed, Canceled
        public decimal Total { set; get; }


    }
}
