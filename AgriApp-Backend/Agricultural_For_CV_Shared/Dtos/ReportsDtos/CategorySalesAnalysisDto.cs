using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Agricultural_For_CV_Shared.Dtos.ReportsDtos
{
    public class CategorySalesAnalysisDto
    {
        public string CategoryName { get; set; } = string.Empty;
        public int TotalOrdersCount { get; set; }
        public int TotalUnitsSold { get; set; }
        public decimal TotalRevenue { get; set; }
    }
}
