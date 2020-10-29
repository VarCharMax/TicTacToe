using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace TicTacToe.Migrations
{
    public partial class FBUser_New1 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_UserModel_FBUserModel_FbUserId",
                table: "UserModel");

            migrationBuilder.DropTable(
                name: "FBUserModel");

            migrationBuilder.DropIndex(
                name: "IX_UserModel_FbUserId",
                table: "UserModel");

            migrationBuilder.DropColumn(
                name: "FbUserId",
                table: "UserModel");

            migrationBuilder.CreateTable(
                name: "IdentityUserRole<Guid>",
                columns: table => new
                {
                    UserId = table.Column<Guid>(nullable: false),
                    RoleId = table.Column<Guid>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_IdentityUserRole<Guid>", x => new { x.UserId, x.RoleId });
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "IdentityUserRole<Guid>");

            migrationBuilder.AddColumn<Guid>(
                name: "FbUserId",
                table: "UserModel",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "FBUserModel",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    LoginProvider = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ProviderDisplayName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ProviderKey = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UserId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FBUserModel", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_UserModel_FbUserId",
                table: "UserModel",
                column: "FbUserId");

            migrationBuilder.AddForeignKey(
                name: "FK_UserModel_FBUserModel_FbUserId",
                table: "UserModel",
                column: "FbUserId",
                principalTable: "FBUserModel",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
