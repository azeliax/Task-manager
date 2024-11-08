using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TimetableWPF.Migrations
{
    /// <inheritdoc />
    public partial class AddCategoryColorHexColumn : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "CategoryColorHex",
                table: "Categories",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CategoryColorHex",
                table: "Categories");
        }
    }
}
