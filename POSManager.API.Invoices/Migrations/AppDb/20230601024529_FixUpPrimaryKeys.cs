using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace POSManager.API.Invoices.Migrations.AppDb
{
    /// <inheritdoc />
    public partial class FixUpPrimaryKeys : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_InvoiceItem_Invoice",
                schema: "dbo",
                table: "InvoiceItem");

            migrationBuilder.DropPrimaryKey(
                name: "PK_InvoiceItem",
                schema: "dbo",
                table: "InvoiceItem");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Invoice",
                schema: "dbo",
                table: "Invoice");

            migrationBuilder.RenameIndex(
                name: "IX_InvoiceItem_InvoiceId_TenantId",
                schema: "dbo",
                table: "InvoiceItem",
                newName: "IX_InvoiceItem_InvoiceId");

            migrationBuilder.AlterColumn<DateTime>(
                name: "IssuedDate",
                schema: "dbo",
                table: "Invoice",
                type: "smalldatetime",
                nullable: false,
                defaultValue: new DateTime(2023, 6, 1, 10, 45, 29, 691, DateTimeKind.Local).AddTicks(3406),
                oldClrType: typeof(DateTime),
                oldType: "smalldatetime",
                oldDefaultValue: new DateTime(2023, 5, 22, 16, 17, 37, 737, DateTimeKind.Local).AddTicks(1956));

            migrationBuilder.AddPrimaryKey(
                name: "PK_InvoiceItem",
                schema: "dbo",
                table: "InvoiceItem",
                column: "InvoiceItemId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Invoice",
                schema: "dbo",
                table: "Invoice",
                column: "InvoiceId");

            migrationBuilder.AddForeignKey(
                name: "FK_InvoiceItem_Invoice",
                schema: "dbo",
                table: "InvoiceItem",
                column: "InvoiceId",
                principalSchema: "dbo",
                principalTable: "Invoice",
                principalColumn: "InvoiceId",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_InvoiceItem_Invoice",
                schema: "dbo",
                table: "InvoiceItem");

            migrationBuilder.DropPrimaryKey(
                name: "PK_InvoiceItem",
                schema: "dbo",
                table: "InvoiceItem");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Invoice",
                schema: "dbo",
                table: "Invoice");

            migrationBuilder.RenameIndex(
                name: "IX_InvoiceItem_InvoiceId",
                schema: "dbo",
                table: "InvoiceItem",
                newName: "IX_InvoiceItem_InvoiceId_TenantId");

            migrationBuilder.AlterColumn<DateTime>(
                name: "IssuedDate",
                schema: "dbo",
                table: "Invoice",
                type: "smalldatetime",
                nullable: false,
                defaultValue: new DateTime(2023, 5, 22, 16, 17, 37, 737, DateTimeKind.Local).AddTicks(1956),
                oldClrType: typeof(DateTime),
                oldType: "smalldatetime",
                oldDefaultValue: new DateTime(2023, 6, 1, 10, 45, 29, 691, DateTimeKind.Local).AddTicks(3406));

            migrationBuilder.AddPrimaryKey(
                name: "PK_InvoiceItem",
                schema: "dbo",
                table: "InvoiceItem",
                columns: new[] { "InvoiceItemId", "TenantId" });

            migrationBuilder.AddPrimaryKey(
                name: "PK_Invoice",
                schema: "dbo",
                table: "Invoice",
                columns: new[] { "InvoiceId", "TenantId" });

            migrationBuilder.AddForeignKey(
                name: "FK_InvoiceItem_Invoice",
                schema: "dbo",
                table: "InvoiceItem",
                columns: new[] { "InvoiceId", "TenantId" },
                principalSchema: "dbo",
                principalTable: "Invoice",
                principalColumns: new[] { "InvoiceId", "TenantId" },
                onDelete: ReferentialAction.Cascade);
        }
    }
}
