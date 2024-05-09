using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WebApi_test.Migrations
{
    /// <inheritdoc />
    public partial class changedImageUrl1 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "villas",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "CreatedDate", "ImageUrl" },
                values: new object[] { new DateTime(2024, 5, 8, 8, 3, 16, 619, DateTimeKind.Local).AddTicks(8036), "https://dotnetmastery.com/bluevillaimages/villa3.jpg" });

            migrationBuilder.UpdateData(
                table: "villas",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "CreatedDate", "ImageUrl" },
                values: new object[] { new DateTime(2024, 5, 8, 8, 3, 16, 619, DateTimeKind.Local).AddTicks(8049), "https://dotnetmastery.com/bluevillaimages/villa1.jpg" });

            migrationBuilder.UpdateData(
                table: "villas",
                keyColumn: "Id",
                keyValue: 3,
                columns: new[] { "CreatedDate", "ImageUrl" },
                values: new object[] { new DateTime(2024, 5, 8, 8, 3, 16, 619, DateTimeKind.Local).AddTicks(8051), "https://dotnetmastery.com/bluevillaimages/villa4.jpg" });

            migrationBuilder.UpdateData(
                table: "villas",
                keyColumn: "Id",
                keyValue: 4,
                columns: new[] { "CreatedDate", "ImageUrl" },
                values: new object[] { new DateTime(2024, 5, 8, 8, 3, 16, 619, DateTimeKind.Local).AddTicks(8053), "https://dotnetmastery.com/bluevillaimages/villa5.jpg" });

            migrationBuilder.UpdateData(
                table: "villas",
                keyColumn: "Id",
                keyValue: 5,
                columns: new[] { "CreatedDate", "ImageUrl" },
                values: new object[] { new DateTime(2024, 5, 8, 8, 3, 16, 619, DateTimeKind.Local).AddTicks(8054), "https://dotnetmastery.com/bluevillaimages/villa2.jpg" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "villas",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "CreatedDate", "ImageUrl" },
                values: new object[] { new DateTime(2024, 5, 8, 4, 0, 48, 767, DateTimeKind.Local).AddTicks(2977), "https://dotnetmasteryimages.com/bluevillaimages/villa3.jpg" });

            migrationBuilder.UpdateData(
                table: "villas",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "CreatedDate", "ImageUrl" },
                values: new object[] { new DateTime(2024, 5, 8, 4, 0, 48, 767, DateTimeKind.Local).AddTicks(2990), "https://dotnetmasteryimages.com/bluevillaimages/villa1.jpg" });

            migrationBuilder.UpdateData(
                table: "villas",
                keyColumn: "Id",
                keyValue: 3,
                columns: new[] { "CreatedDate", "ImageUrl" },
                values: new object[] { new DateTime(2024, 5, 8, 4, 0, 48, 767, DateTimeKind.Local).AddTicks(2991), "https://dotnetmasteryimages.com/bluevillaimages/villa4.jpg" });

            migrationBuilder.UpdateData(
                table: "villas",
                keyColumn: "Id",
                keyValue: 4,
                columns: new[] { "CreatedDate", "ImageUrl" },
                values: new object[] { new DateTime(2024, 5, 8, 4, 0, 48, 767, DateTimeKind.Local).AddTicks(2993), "https://dotnetmasteryimages.com/bluevillaimages/villa5.jpg" });

            migrationBuilder.UpdateData(
                table: "villas",
                keyColumn: "Id",
                keyValue: 5,
                columns: new[] { "CreatedDate", "ImageUrl" },
                values: new object[] { new DateTime(2024, 5, 8, 4, 0, 48, 767, DateTimeKind.Local).AddTicks(2994), "https://dotnetmasteryimages.com/bluevillaimages/villa2.jpg" });
        }
    }
}
