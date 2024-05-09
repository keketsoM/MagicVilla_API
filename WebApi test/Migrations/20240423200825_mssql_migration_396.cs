using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WebApi_test.Migrations
{
    /// <inheritdoc />
    public partial class mssql_migration_396 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_villaNumbers_villas_VillaNumberId",
                table: "villaNumbers");

            migrationBuilder.RenameColumn(
                name: "VillaNumberId",
                table: "villaNumbers",
                newName: "VillaId");

            migrationBuilder.RenameIndex(
                name: "IX_villaNumbers_VillaNumberId",
                table: "villaNumbers",
                newName: "IX_villaNumbers_VillaId");

            migrationBuilder.UpdateData(
                table: "villas",
                keyColumn: "Id",
                keyValue: 1,
                column: "CreatedDate",
                value: new DateTime(2024, 4, 23, 22, 8, 23, 88, DateTimeKind.Local).AddTicks(7437));

            migrationBuilder.UpdateData(
                table: "villas",
                keyColumn: "Id",
                keyValue: 2,
                column: "CreatedDate",
                value: new DateTime(2024, 4, 23, 22, 8, 23, 88, DateTimeKind.Local).AddTicks(7448));

            migrationBuilder.UpdateData(
                table: "villas",
                keyColumn: "Id",
                keyValue: 3,
                column: "CreatedDate",
                value: new DateTime(2024, 4, 23, 22, 8, 23, 88, DateTimeKind.Local).AddTicks(7450));

            migrationBuilder.UpdateData(
                table: "villas",
                keyColumn: "Id",
                keyValue: 4,
                column: "CreatedDate",
                value: new DateTime(2024, 4, 23, 22, 8, 23, 88, DateTimeKind.Local).AddTicks(7452));

            migrationBuilder.UpdateData(
                table: "villas",
                keyColumn: "Id",
                keyValue: 5,
                column: "CreatedDate",
                value: new DateTime(2024, 4, 23, 22, 8, 23, 88, DateTimeKind.Local).AddTicks(7454));

            migrationBuilder.AddForeignKey(
                name: "FK_villaNumbers_villas_VillaId",
                table: "villaNumbers",
                column: "VillaId",
                principalTable: "villas",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_villaNumbers_villas_VillaId",
                table: "villaNumbers");

            migrationBuilder.RenameColumn(
                name: "VillaId",
                table: "villaNumbers",
                newName: "VillaNumberId");

            migrationBuilder.RenameIndex(
                name: "IX_villaNumbers_VillaId",
                table: "villaNumbers",
                newName: "IX_villaNumbers_VillaNumberId");

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

            migrationBuilder.AddForeignKey(
                name: "FK_villaNumbers_villas_VillaNumberId",
                table: "villaNumbers",
                column: "VillaNumberId",
                principalTable: "villas",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
