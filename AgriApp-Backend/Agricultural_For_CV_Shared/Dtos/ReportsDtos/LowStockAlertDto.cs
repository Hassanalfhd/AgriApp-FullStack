using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Agricultural_For_CV_Shared.Dtos.ReportsDtos
{
    public class LowStockAlertDto
    {
        public int ProductId { get; set; }
        public string ProductName { get; set; } = string.Empty;
        public int CurrentStock { get; set; }
        public string CategoryName { get; set; } = string.Empty;
        public string FarmerName { get; set; } = string.Empty;
    }
}
