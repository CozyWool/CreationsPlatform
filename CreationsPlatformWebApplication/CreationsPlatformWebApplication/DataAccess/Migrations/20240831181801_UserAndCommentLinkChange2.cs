using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CreationsPlatformWebApplication.DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class UserAndCommentLinkChange2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "creation_id_fk",
                table: "Comments");

            migrationBuilder.CreateIndex(
                name: "IX_Comments_user_id",
                table: "Comments",
                column: "user_id");

            migrationBuilder.AddForeignKey(
                name: "FK_Comments_Creations_creation_id",
                table: "Comments",
                column: "creation_id",
                principalTable: "Creations",
                principalColumn: "id");

            migrationBuilder.AddForeignKey(
                name: "FK_Comments_Users_user_id",
                table: "Comments",
                column: "user_id",
                principalTable: "Users",
                principalColumn: "id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Comments_Creations_creation_id",
                table: "Comments");

            migrationBuilder.DropForeignKey(
                name: "FK_Comments_Users_user_id",
                table: "Comments");

            migrationBuilder.DropIndex(
                name: "IX_Comments_user_id",
                table: "Comments");

            migrationBuilder.AddForeignKey(
                name: "creation_id_fk",
                table: "Comments",
                column: "creation_id",
                principalTable: "Creations",
                principalColumn: "id");
        }
    }
}
