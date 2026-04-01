using Agricultural_For_CV_Shared.Results;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Threading.Tasks;
using Agricultural_For_CV_Shared.Dtos.ReportsDtos;
using Agricultural_For_CV_Shared.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Agricultural_For_CV_Shared.Enums;

namespace Agricultural_For_CV_API.Controllers
{
    /// <summary>
    /// Controller responsible for handling reporting operations for both Farmers and Admins.
    /// Implements high-performance data retrieval and structured logging.
    /// </summary>
    [Route("api/v{version:ApiVersion}/Reports")]
    [ApiController]
    [ApiVersion("1.0")]
    //[Authorize]
    public class ReportsController : ControllerBase
    {
        private readonly IReportService _reportService;
        private readonly ILogger<ReportsController> _logger;

        public ReportsController(IReportService reportService, ILogger<ReportsController> logger)
        {
            _reportService = reportService;
            _logger = logger;
        }

        /// <summary>
        /// Retrieves the monthly financial and sales report for a specific farmer.
        /// Optimized with SARGable queries and Covering Indexes at the database level.
        /// </summary>
        /// <param name="farmerId">The unique identifier of the farmer.</param>
        /// <param name="year">The target year for the report.</param>
        /// <param name="month">The target month for the report.</param>
        /// <returns>A detailed summary of revenue, commission, and items sold.</returns>
        [HttpGet("farmer-sales/{farmerId}")]
        [ProducesResponseType(typeof(FarmerFinancialSummaryDto), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [Role(UserRole.Farmer, UserRole.Admin)]
        public async Task<IActionResult> GetFarmerMonthlySales(int farmerId, [FromQuery] int year, [FromQuery] int month)
        {
            _logger.LogInformation("Processing monthly sales report request for FarmerId: {Id}", farmerId);

            // Fetching report through BLL with Result Pattern handling
            var result = await _reportService.GetFarmerFinancialStatusAsync(farmerId, year, month);

            if (!result.IsSuccess)
            {
                _logger.LogWarning("Failed to retrieve report for FarmerId: {Id}. Reason: {Error}", farmerId, result.Error);
                return NotFound(new { error = result.Error });
            }

            // Return 200 OK with the DTO payload
            return Ok(result.Data);
        }

        /// <summary>
        /// Retrieves the top-performing farmers based on total revenue generated.
        /// Accessible by Admin users to monitor platform growth.
        /// </summary>
        /// <param name="count">Number of top farmers to retrieve (Default is 5).</param>
        /// <returns>A list of top farmers with their respective sales performance metrics.</returns>
        [HttpGet("admin/top-farmers")]
        [Role(UserRole.Admin)]
        [ProducesResponseType(typeof(IEnumerable<TopFarmerDto>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> GetTopFarmers([FromQuery] int count = 5)
        {
            _logger.LogInformation("Admin requested top {Count} performing farmers report.", count);

            var result = await _reportService.GetTopPerformingFarmersAsync(count);

            if (!result.IsSuccess)
            {
                _logger.LogError("Admin report failed: {Error}", result.Error);
                return BadRequest(new { error = result.Error });
            }

            return Ok(result.Data);
        }


        /// <summary>
        /// Retrieves the Month-over-Month (MoM) sales growth analysis.
        /// Uses SQL Window Functions (LAG) for high-performance trend analysis.
        /// </summary>
        [Role(UserRole.Admin)]
        [HttpGet("admin/sales-growth")]
        [ProducesResponseType(typeof(IEnumerable<SalesGrowthDto>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetSalesGrowth()
        {
            var result = await _reportService.GetPlatformSalesGrowthAsync();

            if (!result.IsSuccess)
                return BadRequest(new { error = result.Error });

            return Ok(result.Data);
        }

        /// <summary>
        /// Retrieves products with low stock levels.
        /// Can be filtered by FarmerId or used by Admin to see all alerts.
        /// </summary>
        [HttpGet("stock-alerts")]
        [Role(UserRole.Admin, UserRole.Farmer)]
        [ProducesResponseType(typeof(IEnumerable<LowStockAlertDto>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetLowStockAlerts([FromQuery] int? farmerId, [FromQuery] int threshold = 10)
        {
            var result = await _reportService.GetLowStockAlertsAsync(farmerId, threshold);

            if (!result.IsSuccess)
                return BadRequest(new { error = result.Error });

            return Ok(result.Data);
        }

        /// <summary>
        /// Provides a business intelligence (BI) report on sales distribution across different categories.
        /// Useful for Admin to identify the most profitable product categories.
        /// </summary>
        /// <returns>A list of categories with total revenue, units sold, and order counts.</returns>
        [HttpGet("admin/category-analysis")]
        [Role(UserRole.Admin)]
        [ProducesResponseType(typeof(IEnumerable<CategorySalesAnalysisDto>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> GetCategoryAnalysis()
        {
            _logger.LogInformation("Admin requested Category Sales Analysis report.");

            var result = await _reportService.GetCategorySalesAnalysisAsync();

            if (!result.IsSuccess)
            {
                return BadRequest(new { error = result.Error });
            }

            return Ok(result.Data);
        }

    }
}