using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CreationsPlatformWebApplication.DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class AddedRatingColumnToComment : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "rating",
                table: "Creations",
                newName: "total_rating");

            migrationBuilder.AddColumn<int>(
                name: "rating",
                table: "Comments",
                type: "integer",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "rating",
                table: "Comments");

            migrationBuilder.RenameColumn(
                name: "total_rating",
                table: "Creations",
                newName: "rating");
        }
    }
}
