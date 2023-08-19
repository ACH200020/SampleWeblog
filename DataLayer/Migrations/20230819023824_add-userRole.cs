using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DataLayer.Migrations
{
    /// <inheritdoc />
    public partial class adduserRole : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Family",
                table: "Users");

            migrationBuilder.RenameColumn(
                name: "UserRole",
                table: "Users",
                newName: "UserRoleId");

            migrationBuilder.RenameColumn(
                name: "Name",
                table: "Users",
                newName: "FullName");

            migrationBuilder.CreateTable(
                name: "UserRoles",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<int>(type: "int", nullable: false),
                    Admin = table.Column<bool>(type: "bit", nullable: false),
                    User = table.Column<bool>(type: "bit", nullable: false),
                    Writer = table.Column<bool>(type: "bit", nullable: false),
                    Downloader = table.Column<bool>(type: "bit", nullable: false),
                    Comment = table.Column<bool>(type: "bit", nullable: false),
                    CreationDate = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserRoles", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Users_UserRoleId",
                table: "Users",
                column: "UserRoleId");

            migrationBuilder.AddForeignKey(
                name: "FK_Users_UserRoles_UserRoleId",
                table: "Users",
                column: "UserRoleId",
                principalTable: "UserRoles",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Users_UserRoles_UserRoleId",
                table: "Users");

            migrationBuilder.DropTable(
                name: "UserRoles");

            migrationBuilder.DropIndex(
                name: "IX_Users_UserRoleId",
                table: "Users");

            migrationBuilder.RenameColumn(
                name: "UserRoleId",
                table: "Users",
                newName: "UserRole");

            migrationBuilder.RenameColumn(
                name: "FullName",
                table: "Users",
                newName: "Name");

            migrationBuilder.AddColumn<string>(
                name: "Family",
                table: "Users",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }
    }
}
