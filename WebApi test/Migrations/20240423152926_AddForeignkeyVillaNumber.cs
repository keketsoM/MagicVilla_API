using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WebApi_test.Migrations
{
    /// <inheritdoc />
    public partial class AddForeignkeyVillaNumber : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "VillaNumberId",
                table: "villaNumbers",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.UpdateData(
                table: "villas",
                keyColumn: "Id",
                keyValue: 1,
                column: "CreatedDate",
                value: new DateTime(2024, 4, 23, 17, 29, 25, 794, DateTimeKind.Local).AddTicks(277));

            migrationBuilder.UpdateData(
                table: "villas",
                keyColumn: "Id",
                keyValue: 2,
                column: "CreatedDate",
                value: new DateTime(2024, 4, 23, 17, 29, 25, 794, DateTimeKind.Local).AddTicks(291));

            migrationBuilder.UpdateData(
                table: "villas",
                keyColumn: "Id",
                keyValue: 3,
                column: "CreatedDate",
                value: new DateTime(2024, 4, 23, 17, 29, 25, 794, DateTimeKind.Local).AddTicks(293));

            migrationBuilder.UpdateData(
                table: "villas",
                keyColumn: "Id",
                keyValue: 4,
                column: "CreatedDate",
                value: new DateTime(2024, 4, 23, 17, 29, 25, 794, DateTimeKind.Local).AddTicks(294));

            migrationBuilder.UpdateData(
                table: "villas",
                keyColumn: "Id",
                keyValue: 5,
                column: "CreatedDate",
                value: new DateTime(2024, 4, 23, 17, 29, 25, 794, DateTimeKind.Local).AddTicks(296));

            migrationBuilder.CreateIndex(
                name: "IX_villaNumbers_VillaNumberId",
                table: "villaNumbers",
                column: "VillaNumberId");

            migrationBuilder.AddForeignKey(
                name: "FK_villaNumbers_villas_VillaNumberId",
                table: "villaNumbers",
                column: "VillaNumberId",
                principalTable: "villas",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_villaNumbers_villas_VillaNumberId",
                table: "villaNumbers");

            migrationBuilder.DropIndex(
                name: "IX_villaNumbers_VillaNumberId",
                table: "villaNumbers");

            migrationBuilder.DropColumn(
                name: "VillaNumberId",
                table: "villaNumbers");

            migrationBuilder.UpdateData(
                table: "villas",
                keyColumn: "Id",
                keyValue: 1,
                column: "CreatedDate",
                value: new DateTime(2024, 4, 22, 19, 18, 18, 452, DateTimeKind.Local).AddTicks(2459));

            migrationBuilder.UpdateData(
                table: "villas",
                keyColumn: "Id",
                keyValue: 2,
                column: "CreatedDate",
                value: new DateTime(2024, 4, 22, 19, 18, 18, 452, DateTimeKind.Local).AddTicks(2473));

            migrationBuilder.UpdateData(
                table: "villas",
                keyColumn: "Id",
                keyValue: 3,
                column: "CreatedDate",
                value: new DateTime(2024, 4, 22, 19, 18, 18, 452, DateTimeKind.Local).AddTicks(2475));

            migrationBuilder.UpdateData(
                table: "villas",
                keyColumn: "Id",
                keyValue: 4,
                column: "CreatedDate",
                value: new DateTime(2024, 4, 22, 19, 18, 18, 452, DateTimeKind.Local).AddTicks(2477));

            migrationBuilder.UpdateData(
                table: "villas",
                keyColumn: "Id",
                keyValue: 5,
                column: "CreatedDate",
                value: new DateTime(2024, 4, 22, 19, 18, 18, 452, DateTimeKind.Local).AddTicks(2478));
        }
    }
}
