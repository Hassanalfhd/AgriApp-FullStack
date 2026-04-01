using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Agricultural_For_CV_DAL.Migrations
{
    /// <inheritdoc />
    public partial class AddPhoneAndIsActiveToUser : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Phone",
                table: "Users",
                type: "nvarchar(14)",
                maxLength: 14,
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "isActive",
                table: "Users",
                type: "bit",
                nullable: false,
                defaultValue: true); // أو false حسب ما تريد
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Phone",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "isActive",
                table: "Users");
        }

    }
}
