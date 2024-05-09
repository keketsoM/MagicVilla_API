using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WebApi_test.Migrations
{
    /// <inheritdoc />
    public partial class changedImageUrl : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
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

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "villas",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "CreatedDate", "ImageUrl" },
                values: new object[] { new DateTime(2024, 5, 3, 10, 19, 18, 579, DateTimeKind.Local).AddTicks(3436), "https://dotnetmasteryimages.blob.core.windows.net/bluevillaimages/villa3.jpg" });

            migrationBuilder.UpdateData(
                table: "villas",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "CreatedDate", "ImageUrl" },
                values: new object[] { new DateTime(2024, 5, 3, 10, 19, 18, 579, DateTimeKind.Local).AddTicks(3448), "https://dotnetmasteryimages.blob.core.windows.net/bluevillaimages/villa1.jpg" });

            migrationBuilder.UpdateData(
                table: "villas",
                keyColumn: "Id",
                keyValue: 3,
                columns: new[] { "CreatedDate", "ImageUrl" },
                values: new object[] { new DateTime(2024, 5, 3, 10, 19, 18, 579, DateTimeKind.Local).AddTicks(3450), "https://dotnetmasteryimages.blob.core.windows.net/bluevillaimages/villa4.jpg" });

            migrationBuilder.UpdateData(
                table: "villas",
                keyColumn: "Id",
                keyValue: 4,
                columns: new[] { "CreatedDate", "ImageUrl" },
                values: new object[] { new DateTime(2024, 5, 3, 10, 19, 18, 579, DateTimeKind.Local).AddTicks(3452), "https://dotnetmasteryimages.blob.core.windows.net/bluevillaimages/villa5.jpg" });

            migrationBuilder.UpdateData(
                table: "villas",
                keyColumn: "Id",
                keyValue: 5,
                columns: new[] { "CreatedDate", "ImageUrl" },
                values: new object[] { new DateTime(2024, 5, 3, 10, 19, 18, 579, DateTimeKind.Local).AddTicks(3453), "https://dotnetmasteryimages.blob.core.windows.net/bluevillaimages/villa2.jpg" });
        }
    }
}
