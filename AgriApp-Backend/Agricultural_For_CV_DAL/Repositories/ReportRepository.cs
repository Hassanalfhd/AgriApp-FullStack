using Dapper;
using Microsoft.EntityFrameworkCore;
using System.Data;
using System.Collections.Generic;
using System.Threading.Tasks;
using Agricultural_For_CV_DAL.Interfaces;
using Agricultural_For_CV_Shared.Dtos.ReportsDtos;

namespace Agricultural_For_CV_DAL.Repositories
{
    public class ReportRepository : IReportRepository
    {
        private readonly AppDbContext _context;

        public ReportRepository(AppDbContext context)
        {
            _context = context;
        }

        /// <summary>
        /// Retrieves the top performing farmers using Dapper for high-performance execution.
        /// Reuses the existing EF Core database connection to optimize resource usage.
        /// </summary>
        public async Task<IEnumerable<TopFarmerDto>> GetTopFarmersAsync(int count, DateTime startDate, DateTime endDate)
        {
            // Extract the underlying DbConnection from Entity Framework Core
            var connection = _context.Database.GetDbConnection();

            var parameters = new
            {
                TopCount = count,
                StartDate = startDate,
                EndDate = endDate
            };

            // Dapper execution using the shared connection
            return await connection.QueryAsync<TopFarmerDto>(
                "sp_GetTopFarmersReport",
                parameters,
                commandType: CommandType.StoredProcedure
            );
        }

        /// <summary>
        /// Retrieves monthly sales details for a farmer.
        /// Optimized with SARGable parameters to ensure Index Seek at the database level.
        /// </summary>
        public async Task<IEnumerable<FarmerMonthlySalesDto>> GetMonthlySalesReportAsync(int farmerId, DateTime startDate, DateTime endDate)
        {
            var connection = _context.Database.GetDbConnection();

            var parameters = new
            {
                FarmerId = farmerId,
                StartDate = startDate,
                EndDate = endDate
            };

            return await connection.QueryAsync<FarmerMonthlySalesDto>(
                "sp_GetFarmerMonthlySalesReport",
                parameters,
                commandType: CommandType.StoredProcedure
            );
        }


        public async Task<IEnumerable<SalesGrowthDto>> GetSalesGrowthReportAsync()
        {
            var connection = _context.Database.GetDbConnection();

            return await connection.QueryAsync<SalesGrowthDto>(
                "sp_GetSalesGrowthReport",
                commandType: CommandType.StoredProcedure
            );
        }

        public async Task<IEnumerable<LowStockAlertDto>> GetLowStockAlertsAsync(int? farmerId, int threshold)
        {
            var connection = _context.Database.GetDbConnection();
            var parameters = new { FarmerId = farmerId, Threshold = threshold };

            return await connection.QueryAsync<LowStockAlertDto>(
                "sp_GetLowStockAlerts",
                parameters,
                commandType: CommandType.StoredProcedure
            );
        }
        public async Task<IEnumerable<CategorySalesAnalysisDto>> GetCategorySalesAnalysisAsync()
        {
            var connection = _context.Database.GetDbConnection();

            return await connection.QueryAsync<CategorySalesAnalysisDto>(
                "sp_GetCategorySalesAnalysis",
                commandType: CommandType.StoredProcedure
            );
        }
    }
}