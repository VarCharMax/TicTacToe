using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace TicTacToe.Migrations
{
    public partial class FBUser_New : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "FbUserId",
                table: "UserModel",
                nullable: true);

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

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_UserModel_FBUserModel_FbUserId",
                table: "UserModel");

            migrationBuilder.DropIndex(
                name: "IX_UserModel_FbUserId",
                table: "UserModel");

            migrationBuilder.DropColumn(
                name: "FbUserId",
                table: "UserModel");
        }
    }
}
