using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Agricultural_For_CV_Shared.Dtos.ReportsDtos
{
    public class SalesGrowthDto
    {
        public int SalesYear { get; set; }
        public int SalesMonth { get; set; }
        public decimal CurrentMonthRevenue { get; set; }
        public decimal PreviousMonthRevenue { get; set; }
        public decimal GrowthPercentage { get; set; }
    }
}
