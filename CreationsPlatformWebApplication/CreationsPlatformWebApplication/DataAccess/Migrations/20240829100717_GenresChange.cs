using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace CreationsPlatformWebApplication.DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class GenresChange : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "genre",
                table: "Creations");

            migrationBuilder.CreateTable(
                name: "Genres",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    name = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("Genres_pkey", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "CreationEntityGenreEntity",
                columns: table => new
                {
                    CreationsId = table.Column<int>(type: "integer", nullable: false),
                    GenresId = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CreationEntityGenreEntity", x => new { x.CreationsId, x.GenresId });
                    table.ForeignKey(
                        name: "FK_CreationEntityGenreEntity_Creations_CreationsId",
                        column: x => x.CreationsId,
                        principalTable: "Creations",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_CreationEntityGenreEntity_Genres_GenresId",
                        column: x => x.GenresId,
                        principalTable: "Genres",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_CreationEntityGenreEntity_GenresId",
                table: "CreationEntityGenreEntity",
                column: "GenresId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CreationEntityGenreEntity");

            migrationBuilder.DropTable(
                name: "Genres");

            migrationBuilder.AddColumn<List<string>>(
                name: "genre",
                table: "Creations",
                type: "character varying[]",
                nullable: false);
        }
    }
}
