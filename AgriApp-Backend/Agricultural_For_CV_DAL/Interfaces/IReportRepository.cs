using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Agricultural_For_CV_Shared.Dtos.ReportsDtos;

namespace Agricultural_For_CV_DAL.Interfaces
{
    public interface IReportRepository
    {
        Task<IEnumerable<TopFarmerDto>> GetTopFarmersAsync(int count, DateTime startDate, DateTime endDate);
        Task<IEnumerable<FarmerMonthlySalesDto>> GetMonthlySalesReportAsync(int farmerId, DateTime startDate, DateTime endDate);
        Task<IEnumerable<CategorySalesAnalysisDto>> GetCategorySalesAnalysisAsync();
        Task<IEnumerable<SalesGrowthDto>> GetSalesGrowthReportAsync();

        Task<IEnumerable<LowStockAlertDto>> GetLowStockAlertsAsync(int? farmerId, int threshold);
    }
}
