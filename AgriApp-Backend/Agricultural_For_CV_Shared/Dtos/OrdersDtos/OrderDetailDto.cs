using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Agricultural_For_CV_Shared.Enums;

namespace Agricultural_For_CV_Shared.Dtos.OrdersDtos
{
    public class OrderDetailDto
    {
        public int OrderItemId { get; set; }
        public int OrderId { get; set; }
        public int ProductId { get; set; }
        public string? ProductName { get; set; }

        public int FarmerId { get; set; }
        public string? FarmerName { get; set; }

        public int Quantity { get; set; }
        public string Status { get; set; }
        public decimal UnitPrice { get; set; }
        public decimal Total { get; set; }
    }



    public class FarmerOrderDto
    {
        public int OrderItemId { get; set; }
        public string? ProductName { get; set; }
        public int Quantity { get; set; }
        public string? CustomerName { get; set; }
        public string? Status { get; set; }
        public DateTime? OrderDate { get; set; }
        public decimal UnitPrice { get; set; }
        public decimal Total{ get; set; }
    }

    public class OrderDetailRequestDto
    {
        public int ProductId { get; set; }
        public int Quantity { get; set; }
        public ItemStatus Status { get; set; }
    }
}
