using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WebApi_test.Migrations
{
    /// <inheritdoc />
    public partial class addLocalimagePath : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "ImageLocalPath",
                table: "villas",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.UpdateData(
                table: "villas",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "CreatedDate", "ImageLocalPath" },
                values: new object[] { new DateTime(2024, 6, 15, 1, 8, 30, 295, DateTimeKind.Local).AddTicks(6435), null });

            migrationBuilder.UpdateData(
                table: "villas",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "CreatedDate", "ImageLocalPath" },
                values: new object[] { new DateTime(2024, 6, 15, 1, 8, 30, 295, DateTimeKind.Local).AddTicks(6458), null });

            migrationBuilder.UpdateData(
                table: "villas",
                keyColumn: "Id",
                keyValue: 3,
                columns: new[] { "CreatedDate", "ImageLocalPath" },
                values: new object[] { new DateTime(2024, 6, 15, 1, 8, 30, 295, DateTimeKind.Local).AddTicks(6460), null });

            migrationBuilder.UpdateData(
                table: "villas",
                keyColumn: "Id",
                keyValue: 4,
                columns: new[] { "CreatedDate", "ImageLocalPath" },
                values: new object[] { new DateTime(2024, 6, 15, 1, 8, 30, 295, DateTimeKind.Local).AddTicks(6462), null });

            migrationBuilder.UpdateData(
                table: "villas",
                keyColumn: "Id",
                keyValue: 5,
                columns: new[] { "CreatedDate", "ImageLocalPath" },
                values: new object[] { new DateTime(2024, 6, 15, 1, 8, 30, 295, DateTimeKind.Local).AddTicks(6464), null });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ImageLocalPath",
                table: "villas");

            migrationBuilder.UpdateData(
                table: "villas",
                keyColumn: "Id",
                keyValue: 1,
                column: "CreatedDate",
                value: new DateTime(2024, 6, 3, 19, 37, 54, 759, DateTimeKind.Local).AddTicks(5101));

            migrationBuilder.UpdateData(
                table: "villas",
                keyColumn: "Id",
                keyValue: 2,
                column: "CreatedDate",
                value: new DateTime(2024, 6, 3, 19, 37, 54, 759, DateTimeKind.Local).AddTicks(5118));

            migrationBuilder.UpdateData(
                table: "villas",
                keyColumn: "Id",
                keyValue: 3,
                column: "CreatedDate",
                value: new DateTime(2024, 6, 3, 19, 37, 54, 759, DateTimeKind.Local).AddTicks(5120));

            migrationBuilder.UpdateData(
                table: "villas",
                keyColumn: "Id",
                keyValue: 4,
                column: "CreatedDate",
                value: new DateTime(2024, 6, 3, 19, 37, 54, 759, DateTimeKind.Local).AddTicks(5140));

            migrationBuilder.UpdateData(
                table: "villas",
                keyColumn: "Id",
                keyValue: 5,
                column: "CreatedDate",
                value: new DateTime(2024, 6, 3, 19, 37, 54, 759, DateTimeKind.Local).AddTicks(5141));
        }
    }
}
