using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace InternetBanking.Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class FixProdcuts : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CuentasAhorro_ProductosFinancieros_IdProductoFinanciero",
                table: "CuentasAhorro");

            migrationBuilder.DropColumn(
                name: "TipoProducto",
                table: "ProductosFinancieros");

            migrationBuilder.AddColumn<int>(
                name: "ProductoFinancieroId",
                table: "CuentasAhorro",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_CuentasAhorro_ProductoFinancieroId",
                table: "CuentasAhorro",
                column: "ProductoFinancieroId");

            migrationBuilder.AddForeignKey(
                name: "FK_CuentasAhorro_ProductoFinanciero",
                table: "CuentasAhorro",
                column: "IdProductoFinanciero",
                principalTable: "ProductosFinancieros",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_CuentasAhorro_ProductosFinancieros_ProductoFinancieroId",
                table: "CuentasAhorro",
                column: "ProductoFinancieroId",
                principalTable: "ProductosFinancieros",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CuentasAhorro_ProductoFinanciero",
                table: "CuentasAhorro");

            migrationBuilder.DropForeignKey(
                name: "FK_CuentasAhorro_ProductosFinancieros_ProductoFinancieroId",
                table: "CuentasAhorro");

            migrationBuilder.DropIndex(
                name: "IX_CuentasAhorro_ProductoFinancieroId",
                table: "CuentasAhorro");

            migrationBuilder.DropColumn(
                name: "ProductoFinancieroId",
                table: "CuentasAhorro");

            migrationBuilder.AddColumn<int>(
                name: "TipoProducto",
                table: "ProductosFinancieros",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddForeignKey(
                name: "FK_CuentasAhorro_ProductosFinancieros_IdProductoFinanciero",
                table: "CuentasAhorro",
                column: "IdProductoFinanciero",
                principalTable: "ProductosFinancieros",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
