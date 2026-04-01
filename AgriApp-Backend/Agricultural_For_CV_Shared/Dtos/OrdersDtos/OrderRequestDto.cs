using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Agricultural_For_CV_Shared.Dtos.OrdersDtos
{
    public class OrderRequestDto
    {
        public int UserId { get; set; }
        public List<OrderDetailRequestDto> Items { get; set; }
    }

}
