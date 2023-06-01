using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace POSManager.API.Invoices.Migrations.AppDb
{
    /// <inheritdoc />
    public partial class ConvertTablesToNubeSync : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "ClusteredIndex",
                schema: "dbo",
                table: "InvoiceItem",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<DateTimeOffset>(
                name: "CreatedAt",
                schema: "dbo",
                table: "InvoiceItem",
                type: "datetimeoffset",
                nullable: false,
                defaultValue: new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)));

            migrationBuilder.AddColumn<DateTimeOffset>(
                name: "DeletedAt",
                schema: "dbo",
                table: "InvoiceItem",
                type: "datetimeoffset",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Id",
                schema: "dbo",
                table: "InvoiceItem",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<DateTimeOffset>(
                name: "ServerUpdatedAt",
                schema: "dbo",
                table: "InvoiceItem",
                type: "datetimeoffset",
                nullable: false,
                defaultValue: new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)));

            migrationBuilder.AddColumn<DateTimeOffset>(
                name: "UpdatedAt",
                schema: "dbo",
                table: "InvoiceItem",
                type: "datetimeoffset",
                nullable: false,
                defaultValue: new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)));

            migrationBuilder.AddColumn<string>(
                name: "UserId",
                schema: "dbo",
                table: "InvoiceItem",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AlterColumn<DateTime>(
                name: "IssuedDate",
                schema: "dbo",
                table: "Invoice",
                type: "smalldatetime",
                nullable: false,
                defaultValue: new DateTime(2023, 6, 1, 11, 43, 29, 749, DateTimeKind.Local).AddTicks(9852),
                oldClrType: typeof(DateTime),
                oldType: "smalldatetime",
                oldDefaultValue: new DateTime(2023, 6, 1, 10, 45, 29, 691, DateTimeKind.Local).AddTicks(3406));

            migrationBuilder.AddColumn<int>(
                name: "ClusteredIndex",
                schema: "dbo",
                table: "Invoice",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<DateTimeOffset>(
                name: "CreatedAt",
                schema: "dbo",
                table: "Invoice",
                type: "datetimeoffset",
                nullable: false,
                defaultValue: new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)));

            migrationBuilder.AddColumn<DateTimeOffset>(
                name: "DeletedAt",
                schema: "dbo",
                table: "Invoice",
                type: "datetimeoffset",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Id",
                schema: "dbo",
                table: "Invoice",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<DateTimeOffset>(
                name: "ServerUpdatedAt",
                schema: "dbo",
                table: "Invoice",
                type: "datetimeoffset",
                nullable: false,
                defaultValue: new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)));

            migrationBuilder.AddColumn<DateTimeOffset>(
                name: "UpdatedAt",
                schema: "dbo",
                table: "Invoice",
                type: "datetimeoffset",
                nullable: false,
                defaultValue: new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)));

            migrationBuilder.AddColumn<string>(
                name: "UserId",
                schema: "dbo",
                table: "Invoice",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "Operations",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    ClusteredIndex = table.Column<int>(type: "int", nullable: false),
                    InstallationId = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ProcessingType = table.Column<byte>(type: "tinyint", nullable: false),
                    ServerUpdatedAt = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false),
                    UserId = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatedAt = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false),
                    ItemId = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    OldValue = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Property = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    TableName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Type = table.Column<byte>(type: "tinyint", nullable: false),
                    Value = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Operations", x => x.Id);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Operations");

            migrationBuilder.DropColumn(
                name: "ClusteredIndex",
                schema: "dbo",
                table: "InvoiceItem");

            migrationBuilder.DropColumn(
                name: "CreatedAt",
                schema: "dbo",
                table: "InvoiceItem");

            migrationBuilder.DropColumn(
                name: "DeletedAt",
                schema: "dbo",
                table: "InvoiceItem");

            migrationBuilder.DropColumn(
                name: "Id",
                schema: "dbo",
                table: "InvoiceItem");

            migrationBuilder.DropColumn(
                name: "ServerUpdatedAt",
                schema: "dbo",
                table: "InvoiceItem");

            migrationBuilder.DropColumn(
                name: "UpdatedAt",
                schema: "dbo",
                table: "InvoiceItem");

            migrationBuilder.DropColumn(
                name: "UserId",
                schema: "dbo",
                table: "InvoiceItem");

            migrationBuilder.DropColumn(
                name: "ClusteredIndex",
                schema: "dbo",
                table: "Invoice");

            migrationBuilder.DropColumn(
                name: "CreatedAt",
                schema: "dbo",
                table: "Invoice");

            migrationBuilder.DropColumn(
                name: "DeletedAt",
                schema: "dbo",
                table: "Invoice");

            migrationBuilder.DropColumn(
                name: "Id",
                schema: "dbo",
                table: "Invoice");

            migrationBuilder.DropColumn(
                name: "ServerUpdatedAt",
                schema: "dbo",
                table: "Invoice");

            migrationBuilder.DropColumn(
                name: "UpdatedAt",
                schema: "dbo",
                table: "Invoice");

            migrationBuilder.DropColumn(
                name: "UserId",
                schema: "dbo",
                table: "Invoice");

            migrationBuilder.AlterColumn<DateTime>(
                name: "IssuedDate",
                schema: "dbo",
                table: "Invoice",
                type: "smalldatetime",
                nullable: false,
                defaultValue: new DateTime(2023, 6, 1, 10, 45, 29, 691, DateTimeKind.Local).AddTicks(3406),
                oldClrType: typeof(DateTime),
                oldType: "smalldatetime",
                oldDefaultValue: new DateTime(2023, 6, 1, 11, 43, 29, 749, DateTimeKind.Local).AddTicks(9852));
        }
    }
}
