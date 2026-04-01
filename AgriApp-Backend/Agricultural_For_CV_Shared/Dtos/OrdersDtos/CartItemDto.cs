using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Agricultural_For_CV_Shared.Dtos.OrdersDtos
{
    public class CartItemDto
    {
        public int ProductId { get; set; }   // معرف المنتج
        public int Quantity { get; set; }    // الكمية المطلوبة
        public decimal Price { get; set; }   // السعر وقت الإضافة للسلة
        public string FarmerId { get; set; } // معرف المزارع (لكي نعرف من صاحب هذا المنتج)
        public decimal UnitPrice { get; set; }

    }
}
