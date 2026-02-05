using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Pos.Web.Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class AddIsCashOnDeliveryColumnToOrdersTabele : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsCashOnDelivery",
                table: "Orders",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsCashOnDelivery",
                table: "Orders");
        }
    }
}
