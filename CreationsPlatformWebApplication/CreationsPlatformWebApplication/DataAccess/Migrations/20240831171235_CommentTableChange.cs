using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CreationsPlatformWebApplication.DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class CommentTableChange : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "user_id_fk",
                table: "Comments");

            migrationBuilder.DropForeignKey(
                name: "FK_CreationEntityGenreEntity_Creations_CreationsId",
                table: "CreationEntityGenreEntity");

            migrationBuilder.DropIndex(
                name: "IX_Comments_user_id",
                table: "Comments");

            migrationBuilder.RenameColumn(
                name: "CreationsId",
                table: "CreationEntityGenreEntity",
                newName: "CreationEntityId");

            migrationBuilder.AddColumn<int>(
                name: "comment_count",
                table: "Creations",
                type: "integer",
                nullable: false,
                defaultValue: 0);

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

            migrationBuilder.AddForeignKey(
                name: "FK_CreationEntityGenreEntity_Creations_CreationEntityId",
                table: "CreationEntityGenreEntity",
                column: "CreationEntityId",
                principalTable: "Creations",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Comments_Users_UserEntityId",
                table: "Comments");

            migrationBuilder.DropForeignKey(
                name: "FK_CreationEntityGenreEntity_Creations_CreationEntityId",
                table: "CreationEntityGenreEntity");

            migrationBuilder.DropIndex(
                name: "IX_Comments_UserEntityId",
                table: "Comments");

            migrationBuilder.DropColumn(
                name: "comment_count",
                table: "Creations");

            migrationBuilder.DropColumn(
                name: "UserEntityId",
                table: "Comments");

            migrationBuilder.RenameColumn(
                name: "CreationEntityId",
                table: "CreationEntityGenreEntity",
                newName: "CreationsId");

            migrationBuilder.CreateIndex(
                name: "IX_Comments_user_id",
                table: "Comments",
                column: "user_id");

            migrationBuilder.AddForeignKey(
                name: "user_id_fk",
                table: "Comments",
                column: "user_id",
                principalTable: "Users",
                principalColumn: "id");

            migrationBuilder.AddForeignKey(
                name: "FK_CreationEntityGenreEntity_Creations_CreationsId",
                table: "CreationEntityGenreEntity",
                column: "CreationsId",
                principalTable: "Creations",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
