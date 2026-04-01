using Agricultural_For_CV_DAL.Interfaces;
using Agricultural_For_CV_Shared.Dtos.ReportsDtos;
using Agricultural_For_CV_Shared.Interfaces;
using Agricultural_For_CV_Shared.Results;
using Microsoft.Extensions.Logging;

namespace Agricultural_For_CV_BLL.Services
{

    public class ReportService : IReportService
    {
        private readonly IReportRepository _reportRepository;
        private readonly ILogger<ReportService> _logger;
        private const decimal PlatformCommissionRate = 0.05m;

        public ReportService(IReportRepository reportRepository, ILogger<ReportService> logger)
        {
            _reportRepository = reportRepository;
            _logger = logger;
        }

        public async Task<Result<FarmerFinancialSummaryDto>> GetFarmerFinancialStatusAsync(int farmerId, int year, int month)
        {
            _logger.LogInformation("Starting financial report extraction for farmer {FarmerId} for period {Month}/{Year}", farmerId, month, year);

            try
            {
                // تحويل السنة والشهر إلى نطاق زمني SARGable
                // Tarin Year and Month To Range Date ---> SARGable
                DateTime startDate = new DateTime(year, month, 1);
                DateTime endDate = startDate.AddMonths(1).AddSeconds(-1);

                var salesData = await _reportRepository.GetMonthlySalesReportAsync(farmerId, startDate, endDate);

                if (salesData == null || !salesData.Any())
                {
                    _logger.LogWarning("No sales data found for farmer {FarmerId} in the specified period", farmerId);
                    return Result<FarmerFinancialSummaryDto>.Failure($"لا توجد مبيعات مسجلة للمزارع في شهر {month} لعام {year}.");
                }

                decimal totalSales = salesData.Sum(x => x.TotalRevenue);
                decimal commission = totalSales * PlatformCommissionRate;

                var summary = new FarmerFinancialSummaryDto
                {
                    MonthlyData = salesData,
                    GrossRevenue = totalSales,
                    PlatformCommission = commission,
                    NetRevenue = totalSales - commission,
                    ReportDate = startDate.ToString("MMMM yyyy")
                };

                _logger.LogInformation("Report successfully extracted for farmer {FarmerId}. Total Sales: {TotalSales}", farmerId, totalSales);
                return Result<FarmerFinancialSummaryDto>.Success(summary, "تم استخراج التقرير المالي بنجاح.");
            }
            catch (ArgumentOutOfRangeException ex)
            {
                _logger.LogError(ex, "Invalid date format provided for farmer {FarmerId}: {Month}/{Year}", farmerId, month, year);
                return Result<FarmerFinancialSummaryDto>.Failure("التاريخ المدخل غير صحيح.");
            }
            catch (Exception ex)
            {
                _logger.LogCritical(ex, "Unexpected error while processing report for farmer {FarmerId}", farmerId);
                return Result<FarmerFinancialSummaryDto>.Failure("حدث خطأ داخلي أثناء إعداد التقرير المالي.");
            }
        }

        public async Task<Result<IEnumerable<TopFarmerDto>>> GetTopPerformingFarmersAsync(int count)
        {
            _logger.LogInformation("Top {Count} performing farmers report requested by administrator", count);

            try
            {
                if (count <= 0) count = 5;

                DateTime startDate = new DateTime(DateTime.UtcNow.Year, DateTime.UtcNow.Month, 1);
                DateTime endDate = DateTime.UtcNow;

                var topFarmers = await _reportRepository.GetTopFarmersAsync(count, startDate, endDate);

                if (topFarmers == null || !topFarmers.Any())
                {
                    _logger.LogWarning("Top performing farmers report returned no results for period {Start} to {End}", startDate, endDate);
                    return Result<IEnumerable<TopFarmerDto>>.Failure("لا توجد بيانات متاحة لتصنيف المزارعين.");
                }

                _logger.LogInformation("Top farmers list retrieved successfully. Count: {ActualCount}", topFarmers.Count());
                return Result<IEnumerable<TopFarmerDto>>.Success(topFarmers);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to execute top performing farmers report");
                return Result<IEnumerable<TopFarmerDto>>.Failure("فشل في استخراج تقرير أفضل المزارعين.");
            }
        }


        public async Task<Result<IEnumerable<CategorySalesAnalysisDto>>> GetCategorySalesAnalysisAsync()
        {
            try
            {
                _logger.LogInformation("Generating Category-based sales analysis report for Admin.");

                var analysisData = await _reportRepository.GetCategorySalesAnalysisAsync();

                if (analysisData == null || !analysisData.Any())
                {
                    _logger.LogWarning("Category sales analysis returned no data.");
                    return Result<IEnumerable<CategorySalesAnalysisDto>>.Failure("No sales data found to perform category analysis.");
                }

                _logger.LogInformation("Category sales analysis generated successfully with {Count} categories.", analysisData.Count());
                return Result<IEnumerable<CategorySalesAnalysisDto>>.Success(analysisData);
            }
            catch (Exception ex)
            {
                _logger.LogCritical(ex, "Unexpected error during category sales analysis execution.");
                return Result<IEnumerable<CategorySalesAnalysisDto>>.Failure("An internal error occurred while analyzing category sales.");
            }
        }
        public async Task<Result<IEnumerable<SalesGrowthDto>>> GetPlatformSalesGrowthAsync()
        {
            try
            {
                _logger.LogInformation("Generating platform sales growth report.");

                var growthData = await _reportRepository.GetSalesGrowthReportAsync();

                if (growthData == null || !growthData.Any())
                {
                    return Result<IEnumerable<SalesGrowthDto>>.Failure("No sales data available to calculate growth.");
                }

                return Result<IEnumerable<SalesGrowthDto>>.Success(growthData);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while calculating sales growth.");
                return Result<IEnumerable<SalesGrowthDto>>.Failure("Internal server error during growth analysis.");
            }
        }


        public async Task<Result<IEnumerable<LowStockAlertDto>>> GetLowStockAlertsAsync(int? farmerId, int threshold)
        {
            try
            {
                _logger.LogInformation("Checking low stock alerts. Threshold: {Threshold}", threshold);

                var alerts = await _reportRepository.GetLowStockAlertsAsync(farmerId, threshold);

                // here  we can send Notification and do additional things
                // like send Notification if quantity is zero

                return Result<IEnumerable<LowStockAlertDto>>.Success(alerts);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error fetching low stock alerts");
                return Result<IEnumerable<LowStockAlertDto>>.Failure("Failed to retrieve stock alerts.");
            }
        }

    }
}
