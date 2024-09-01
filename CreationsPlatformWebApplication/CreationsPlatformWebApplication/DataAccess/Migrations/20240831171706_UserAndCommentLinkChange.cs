using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CreationsPlatformWebApplication.DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class UserAndCommentLinkChange : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Comments_Users_UserEntityId",
                table: "Comments");

            migrationBuilder.DropIndex(
                name: "IX_Comments_UserEntityId",
                table: "Comments");

            migrationBuilder.DropColumn(
                name: "UserEntityId",
                table: "Comments");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "UserEntityId",
                table: "Comments",
                type: "uuid",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Comments_UserEntityId",
                table: "Comments",
                column: "UserEntityId");

            migrationBuilder.AddForeignKey(
                name: "FK_Comments_Users_UserEntityId",
                table: "Comments",
                column: "UserEntityId",
                principalTable: "Users",
                principalColumn: "id");
        }
    }
}
