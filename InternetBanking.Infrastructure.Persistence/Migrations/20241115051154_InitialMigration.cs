using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace InternetBanking.Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class InitialMigration : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ProductosFinancieros",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    IdentificadorUnico = table.Column<string>(type: "nvarchar(9)", maxLength: 9, nullable: false),
                    IdUsuario = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    NumeroProducto = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    FechaCreacion = table.Column<DateTime>(type: "datetime2", nullable: false),
                    TipoProducto = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProductosFinancieros", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "CuentasAhorro",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    IdentificadorUnico = table.Column<string>(type: "nvarchar(9)", maxLength: 9, nullable: false),
                    NumeroCuenta = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    Balance = table.Column<decimal>(type: "decimal(18,2)", precision: 18, scale: 2, nullable: false),
                    EsPrincipal = table.Column<bool>(type: "bit", nullable: false),
                    IdProductoFinanciero = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CuentasAhorro", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CuentasAhorro_ProductosFinancieros_IdProductoFinanciero",
                        column: x => x.IdProductoFinanciero,
                        principalTable: "ProductosFinancieros",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Prestamos",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    MontoPrestamo = table.Column<decimal>(type: "decimal(18,2)", precision: 18, scale: 2, nullable: false),
                    DeudaRestante = table.Column<decimal>(type: "decimal(18,2)", precision: 18, scale: 2, nullable: false),
                    FechaInicio = table.Column<DateTime>(type: "datetime2", nullable: false),
                    IdProductoFinanciero = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Prestamos", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Prestamos_ProductosFinancieros_IdProductoFinanciero",
                        column: x => x.IdProductoFinanciero,
                        principalTable: "ProductosFinancieros",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "TarjetasCredito",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    IdentificadorUnico = table.Column<string>(type: "nvarchar(9)", maxLength: 9, nullable: false),
                    NumeroTarjeta = table.Column<string>(type: "nvarchar(16)", maxLength: 16, nullable: false),
                    LimiteCredito = table.Column<decimal>(type: "decimal(18,2)", precision: 18, scale: 2, nullable: false),
                    DeudaActual = table.Column<decimal>(type: "decimal(18,2)", precision: 18, scale: 2, nullable: false),
                    IdProductoFinanciero = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TarjetasCredito", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TarjetasCredito_ProductosFinancieros_IdProductoFinanciero",
                        column: x => x.IdProductoFinanciero,
                        principalTable: "ProductosFinancieros",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Beneficiarios",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    IdUsuario = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    IdCuentaBeneficiario = table.Column<int>(type: "int", nullable: false),
                    CuentaBeneficiarioId = table.Column<int>(type: "int", nullable: false),
                    Nombre = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Apellido = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    NumeroCuenta = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Beneficiarios", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Beneficiarios_CuentasAhorro_CuentaBeneficiarioId",
                        column: x => x.CuentaBeneficiarioId,
                        principalTable: "CuentasAhorro",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Transacciones",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    IdUsuario = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    TipoTransaccion = table.Column<int>(type: "int", nullable: false),
                    Monto = table.Column<decimal>(type: "decimal(18,2)", precision: 18, scale: 2, nullable: false),
                    Fecha = table.Column<DateTime>(type: "datetime2", nullable: false),
                    IdCuentaOrigen = table.Column<int>(type: "int", nullable: true),
                    IdCuentaDestino = table.Column<int>(type: "int", nullable: true),
                    IdProductoFinanciero = table.Column<int>(type: "int", nullable: true),
                    ProductoFinancieroId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Transacciones", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Transacciones_CuentasAhorro_IdCuentaDestino",
                        column: x => x.IdCuentaDestino,
                        principalTable: "CuentasAhorro",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Transacciones_CuentasAhorro_IdCuentaOrigen",
                        column: x => x.IdCuentaOrigen,
                        principalTable: "CuentasAhorro",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Transacciones_ProductosFinancieros_ProductoFinancieroId",
                        column: x => x.ProductoFinancieroId,
                        principalTable: "ProductosFinancieros",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AvancesEfectivo",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Monto = table.Column<decimal>(type: "decimal(18,2)", precision: 18, scale: 2, nullable: false),
                    Interes = table.Column<decimal>(type: "decimal(18,2)", precision: 18, scale: 2, nullable: false),
                    FechaAvance = table.Column<DateTime>(type: "datetime2", nullable: false),
                    IdTarjetaCredito = table.Column<int>(type: "int", nullable: false),
                    IdCuentaDestino = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AvancesEfectivo", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AvancesEfectivo_CuentasAhorro_IdCuentaDestino",
                        column: x => x.IdCuentaDestino,
                        principalTable: "CuentasAhorro",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_AvancesEfectivo_TarjetasCredito_IdTarjetaCredito",
                        column: x => x.IdTarjetaCredito,
                        principalTable: "TarjetasCredito",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Pagos",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    IdUsuario = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    TipoPago = table.Column<int>(type: "int", nullable: false),
                    Monto = table.Column<decimal>(type: "decimal(18,2)", precision: 18, scale: 2, nullable: false),
                    Fecha = table.Column<DateTime>(type: "datetime2", nullable: false),
                    IdCuentaPago = table.Column<int>(type: "int", nullable: true),
                    IdBeneficiario = table.Column<int>(type: "int", nullable: true),
                    IdProductoFinanciero = table.Column<int>(type: "int", nullable: true),
                    ProductoFinancieroId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Pagos", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Pagos_Beneficiarios_IdBeneficiario",
                        column: x => x.IdBeneficiario,
                        principalTable: "Beneficiarios",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Pagos_CuentasAhorro_IdCuentaPago",
                        column: x => x.IdCuentaPago,
                        principalTable: "CuentasAhorro",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Pagos_ProductosFinancieros_ProductoFinancieroId",
                        column: x => x.ProductoFinancieroId,
                        principalTable: "ProductosFinancieros",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_AvancesEfectivo_IdCuentaDestino",
                table: "AvancesEfectivo",
                column: "IdCuentaDestino");

            migrationBuilder.CreateIndex(
                name: "IX_AvancesEfectivo_IdTarjetaCredito",
                table: "AvancesEfectivo",
                column: "IdTarjetaCredito");

            migrationBuilder.CreateIndex(
                name: "IX_Beneficiarios_CuentaBeneficiarioId",
                table: "Beneficiarios",
                column: "CuentaBeneficiarioId");

            migrationBuilder.CreateIndex(
                name: "IX_CuentasAhorro_IdProductoFinanciero",
                table: "CuentasAhorro",
                column: "IdProductoFinanciero",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Pagos_IdBeneficiario",
                table: "Pagos",
                column: "IdBeneficiario");

            migrationBuilder.CreateIndex(
                name: "IX_Pagos_IdCuentaPago",
                table: "Pagos",
                column: "IdCuentaPago");

            migrationBuilder.CreateIndex(
                name: "IX_Pagos_ProductoFinancieroId",
                table: "Pagos",
                column: "ProductoFinancieroId");

            migrationBuilder.CreateIndex(
                name: "IX_Prestamos_IdProductoFinanciero",
                table: "Prestamos",
                column: "IdProductoFinanciero",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_TarjetasCredito_IdProductoFinanciero",
                table: "TarjetasCredito",
                column: "IdProductoFinanciero",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Transacciones_IdCuentaDestino",
                table: "Transacciones",
                column: "IdCuentaDestino");

            migrationBuilder.CreateIndex(
                name: "IX_Transacciones_IdCuentaOrigen",
                table: "Transacciones",
                column: "IdCuentaOrigen");

            migrationBuilder.CreateIndex(
                name: "IX_Transacciones_ProductoFinancieroId",
                table: "Transacciones",
                column: "ProductoFinancieroId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AvancesEfectivo");

            migrationBuilder.DropTable(
                name: "Pagos");

            migrationBuilder.DropTable(
                name: "Prestamos");

            migrationBuilder.DropTable(
                name: "Transacciones");

            migrationBuilder.DropTable(
                name: "TarjetasCredito");

            migrationBuilder.DropTable(
                name: "Beneficiarios");

            migrationBuilder.DropTable(
                name: "CuentasAhorro");

            migrationBuilder.DropTable(
                name: "ProductosFinancieros");
        }
    }
}
