using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Agricultural_For_CV_Shared.Dtos.ReportsDtos
{
    public class TopFarmerDto
    {
        public int FarmerId { get; set; }
        public string FarmerName { get; set; } = string.Empty;
        public string FarmerEmail { get; set; } = string.Empty;
        public int TotalOrders { get; set; }
        public int TotalProductsSold { get; set; }
        public decimal TotalRevenue { get; set; }
    }
}
