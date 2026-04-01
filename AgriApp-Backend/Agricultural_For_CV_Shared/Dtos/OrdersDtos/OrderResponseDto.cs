using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Agricultural_For_CV_Shared.Enums;

namespace Agricultural_For_CV_Shared.Dtos.OrdersDtos
{
    public class OrderResponseDto
    {
        public int OrderId { get; set; }
        public string Status { get; set; }
        public decimal TotalAmount { get; set; }
        public DateTime CreatedAt { get; set; }
        public string? FarmerName { get; set; }
        public List<OrderDetailDto> Items { get; set; }
    }
}
