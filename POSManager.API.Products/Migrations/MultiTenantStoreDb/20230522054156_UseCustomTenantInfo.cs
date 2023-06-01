using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace POSManager.API.Products.Migrations.MultiTenantStoreDb
{
    /// <inheritdoc />
    public partial class UseCustomTenantInfo : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "LicenseExpires",
                table: "TenantInfo",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "LicenseLevel",
                table: "TenantInfo",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "LicenseExpires",
                table: "TenantInfo");

            migrationBuilder.DropColumn(
                name: "LicenseLevel",
                table: "TenantInfo");
        }
    }
}
