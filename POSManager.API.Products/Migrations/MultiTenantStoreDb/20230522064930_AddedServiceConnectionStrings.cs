using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace POSManager.API.Products.Migrations.MultiTenantStoreDb
{
    /// <inheritdoc />
    public partial class AddedServiceConnectionStrings : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "ConnectionStringInvoices",
                table: "TenantInfo",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "ConnectionStringProducts",
                table: "TenantInfo",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ConnectionStringInvoices",
                table: "TenantInfo");

            migrationBuilder.DropColumn(
                name: "ConnectionStringProducts",
                table: "TenantInfo");
        }
    }
}
