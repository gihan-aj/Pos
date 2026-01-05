using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Pos.Web.Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class AddIsActiveColumnToProductVariants : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsActive",
                table: "ProductVarients",
                type: "bit",
                nullable: false,
                defaultValue: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsActive",
                table: "ProductVarients");
        }
    }
}
