using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CreationsPlatformWebApplication.DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class GenresDoesntWork : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CreationEntityGenreEntity_Creations_CreationEntityId",
                table: "CreationEntityGenreEntity");

            migrationBuilder.RenameColumn(
                name: "CreationEntityId",
                table: "CreationEntityGenreEntity",
                newName: "CreationsId");

            migrationBuilder.AddForeignKey(
                name: "FK_CreationEntityGenreEntity_Creations_CreationsId",
                table: "CreationEntityGenreEntity",
                column: "CreationsId",
                principalTable: "Creations",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CreationEntityGenreEntity_Creations_CreationsId",
                table: "CreationEntityGenreEntity");

            migrationBuilder.RenameColumn(
                name: "CreationsId",
                table: "CreationEntityGenreEntity",
                newName: "CreationEntityId");

            migrationBuilder.AddForeignKey(
                name: "FK_CreationEntityGenreEntity_Creations_CreationEntityId",
                table: "CreationEntityGenreEntity",
                column: "CreationEntityId",
                principalTable: "Creations",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
