using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Sportswear.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class Add_ProductVariant_SKU_ColorName_ColorHex_Indexes : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_ProductVariants_ProductId",
                table: "ProductVariants");

            migrationBuilder.RenameColumn(
                name: "Color",
                table: "ProductVariants",
                newName: "SKU");

            migrationBuilder.AddColumn<string>(
                name: "ColorHex",
                table: "ProductVariants",
                type: "nvarchar(10)",
                maxLength: 10,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "ColorName",
                table: "ProductVariants",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateIndex(
                name: "IX_ProductVariants_ProductId_ColorName_Size",
                table: "ProductVariants",
                columns: new[] { "ProductId", "ColorName", "Size" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_ProductVariants_SKU",
                table: "ProductVariants",
                column: "SKU",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_ProductVariants_ProductId_ColorName_Size",
                table: "ProductVariants");

            migrationBuilder.DropIndex(
                name: "IX_ProductVariants_SKU",
                table: "ProductVariants");

            migrationBuilder.DropColumn(
                name: "ColorHex",
                table: "ProductVariants");

            migrationBuilder.DropColumn(
                name: "ColorName",
                table: "ProductVariants");

            migrationBuilder.RenameColumn(
                name: "SKU",
                table: "ProductVariants",
                newName: "Color");

            migrationBuilder.CreateIndex(
                name: "IX_ProductVariants_ProductId",
                table: "ProductVariants",
                column: "ProductId");
        }
    }
}
