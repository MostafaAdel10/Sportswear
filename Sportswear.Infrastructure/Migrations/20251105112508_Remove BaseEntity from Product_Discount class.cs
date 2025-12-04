using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Sportswear.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class RemoveBaseEntityfromProduct_Discountclass : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Id",
                table: "Product_Discounts");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "Id",
                table: "Product_Discounts",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }
    }
}
