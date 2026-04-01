using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Agricultural_For_CV_Shared.Dtos.ReportsDtos
{
    public class FarmerMonthlySalesDto
    {
        public string ProductName { get; set; } = string.Empty;
        public int TotalQuantitySold { get; set; }
        public decimal TotalRevenue { get; set; }
        public int NumberOfOrders { get; set; }
    }


    public class FarmerFinancialSummaryDto
    {
        public IEnumerable<FarmerMonthlySalesDto> MonthlyData { get; set; } = new List<FarmerMonthlySalesDto>();
        public decimal GrossRevenue { get; set; }
        public decimal NetRevenue { get; set; }
        public decimal PlatformCommission { get; set; }
        public string ReportDate { get; set; } = string.Empty;
    }
}
