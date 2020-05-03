using Microsoft.EntityFrameworkCore.Migrations;

namespace E_commerce_app.Data.Migrations
{
    public partial class AddQuantityToTransactionProducts : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "Quantity",
                table: "TransactionProduct",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Quantity",
                table: "TransactionProduct");
        }
    }
}
