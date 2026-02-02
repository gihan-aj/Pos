using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Pos.Web.Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class ChangeOrderPaymentsTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsSuccessful",
                table: "OrderPayments");

            migrationBuilder.RenameColumn(
                name: "PaymentStatus",
                table: "Orders",
                newName: "OrderPaymentStatus");

            migrationBuilder.RenameColumn(
                name: "PaymentMethod",
                table: "OrderPayments",
                newName: "Status");

            migrationBuilder.AddColumn<string>(
                name: "GatewayResponse",
                table: "OrderPayments",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "Method",
                table: "OrderPayments",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "Provider",
                table: "OrderPayments",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "GatewayResponse",
                table: "OrderPayments");

            migrationBuilder.DropColumn(
                name: "Method",
                table: "OrderPayments");

            migrationBuilder.DropColumn(
                name: "Provider",
                table: "OrderPayments");

            migrationBuilder.RenameColumn(
                name: "OrderPaymentStatus",
                table: "Orders",
                newName: "PaymentStatus");

            migrationBuilder.RenameColumn(
                name: "Status",
                table: "OrderPayments",
                newName: "PaymentMethod");

            migrationBuilder.AddColumn<bool>(
                name: "IsSuccessful",
                table: "OrderPayments",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }
    }
}
