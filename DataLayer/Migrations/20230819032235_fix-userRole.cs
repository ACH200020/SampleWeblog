using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace DataLayer.Migrations
{
    /// <inheritdoc />
    public partial class fixuserRole : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "UserId",
                table: "UserRoles");

            migrationBuilder.InsertData(
                table: "UserRoles",
                columns: new[] { "Id", "Admin", "Comment", "CreationDate", "Downloader", "User", "Writer" },
                values: new object[,]
                {
                    { 1, true, true, new DateTime(2023, 8, 18, 20, 22, 35, 580, DateTimeKind.Local).AddTicks(4893), true, false, true },
                    { 2, false, true, new DateTime(2023, 8, 18, 20, 22, 35, 580, DateTimeKind.Local).AddTicks(4938), true, false, false }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "UserRoles",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "UserRoles",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.AddColumn<int>(
                name: "UserId",
                table: "UserRoles",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }
    }
}
