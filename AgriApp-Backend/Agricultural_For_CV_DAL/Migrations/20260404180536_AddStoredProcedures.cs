using Microsoft.EntityFrameworkCore.Migrations;
using System.IO;
#nullable disable

namespace Agricultural_For_CV_DAL.Migrations
{
    /// <inheritdoc />
    public partial class AddStoredProcedures : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            var projectPath = Path.GetFullPath(Path.Combine(Directory.GetCurrentDirectory(), @"..\Agricultural_For_CV_DAL"));
            var basePath = Path.Combine(projectPath, "StoredProcedures");
            var files = Directory.GetFiles(basePath, "*.sql");

            foreach (var file in files)
            {
                var sql = File.ReadAllText(file);
                migrationBuilder.Sql(sql);
            }
        }
        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("DROP PROCEDURE IF EXISTS sp_GetTopFarmersReport");
            migrationBuilder.Sql("DROP PROCEDURE IF EXISTS sp_GetSalesGrowthReport");
            migrationBuilder.Sql("DROP PROCEDURE IF EXISTS sp_GetLowStockAlerts");
            migrationBuilder.Sql("DROP PROCEDURE IF EXISTS sp_GetFarmerMonthlySalesReport");
            migrationBuilder.Sql("DROP PROCEDURE IF EXISTS sp_GetCategorySalesAnalysis");
        }

    }
}
