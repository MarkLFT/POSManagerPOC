using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace POSManager.API.Invoices.Migrations.AppDb
{
    /// <inheritdoc />
    public partial class AddInvocieTables : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(
                name: "dbo");

            migrationBuilder.CreateTable(
                name: "Invoice",
                schema: "dbo",
                columns: table => new
                {
                    InvoiceId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    TenantId = table.Column<string>(type: "nvarchar(64)", maxLength: 64, nullable: false),
                    InvoiceNo = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: false),
                    GuestName = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    IssuedDate = table.Column<DateTime>(type: "smalldatetime", nullable: false, defaultValue: new DateTime(2023, 5, 22, 16, 17, 37, 737, DateTimeKind.Local).AddTicks(1956)),
                    TableId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Invoice", x => new { x.InvoiceId, x.TenantId });
                });

            migrationBuilder.CreateTable(
                name: "InvoiceItem",
                schema: "dbo",
                columns: table => new
                {
                    InvoiceItemId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    TenantId = table.Column<string>(type: "nvarchar(64)", maxLength: 64, nullable: false),
                    InvoiceId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    LineNumber = table.Column<int>(type: "int", nullable: false),
                    ProductId = table.Column<int>(type: "int", nullable: true),
                    Description = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Price = table.Column<decimal>(type: "money", nullable: false, defaultValue: 0m),
                    Quantity = table.Column<int>(type: "int", nullable: false, defaultValue: 0)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_InvoiceItem", x => new { x.InvoiceItemId, x.TenantId });
                    table.ForeignKey(
                        name: "FK_InvoiceItem_Invoice",
                        columns: x => new { x.InvoiceId, x.TenantId },
                        principalSchema: "dbo",
                        principalTable: "Invoice",
                        principalColumns: new[] { "InvoiceId", "TenantId" },
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_InvoiceItem_InvoiceId_TenantId",
                schema: "dbo",
                table: "InvoiceItem",
                columns: new[] { "InvoiceId", "TenantId" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "InvoiceItem",
                schema: "dbo");

            migrationBuilder.DropTable(
                name: "Invoice",
                schema: "dbo");
        }
    }
}
