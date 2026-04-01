using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Agricultural_For_CV_Shared.Dtos.ReportsDtos;
using Agricultural_For_CV_Shared.Results;

namespace Agricultural_For_CV_Shared.Interfaces
{
    public interface IReportService
    {
        Task<Result<IEnumerable<TopFarmerDto>>> GetTopPerformingFarmersAsync(int count);

        Task<Result<FarmerFinancialSummaryDto>> GetFarmerFinancialStatusAsync(int farmerId, int year, int month);

        Task<Result<IEnumerable<LowStockAlertDto>>> GetLowStockAlertsAsync(int? farmerId, int threshold);

        Task<Result<IEnumerable<SalesGrowthDto>>> GetPlatformSalesGrowthAsync();

        Task<Result<IEnumerable<CategorySalesAnalysisDto>>> GetCategorySalesAnalysisAsync();
    }
}
