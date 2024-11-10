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
                name: "AvancesEfectivo",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Monto = table.Column<decimal>(type: "decimal(18,2)", precision: 18, scale: 2, nullable: false),
                    Interes = table.Column<decimal>(type: "decimal(18,2)", precision: 18, scale: 2, nullable: false),
                    IdTarjetaCredito = table.Column<int>(type: "int", nullable: false),
                    IdCuentaDestino = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AvancesEfectivo", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Beneficiarios",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    IdUsuario = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IdCuentaBeneficiario = table.Column<int>(type: "int", nullable: false),
                    Nombre = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Apellido = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    NumeroCuenta = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Beneficiarios", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "CuentasAhorro",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    IdentificadorUnico = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    NumeroCuenta = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Balance = table.Column<decimal>(type: "decimal(18,2)", precision: 18, scale: 2, nullable: false),
                    EsPrincipal = table.Column<bool>(type: "bit", nullable: false),
                    IdProductoFinanciero = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CuentasAhorro", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Pagos",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    IdUsuario = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    TipoPago = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Monto = table.Column<decimal>(type: "decimal(18,2)", precision: 18, scale: 2, nullable: false),
                    Fecha = table.Column<DateTime>(type: "datetime2", nullable: false),
                    IdCuentaPago = table.Column<int>(type: "int", nullable: true),
                    IdBeneficiario = table.Column<int>(type: "int", nullable: true),
                    IdProductoFinanciero = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Pagos", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Pagos_Beneficiarios_IdBeneficiario",
                        column: x => x.IdBeneficiario,
                        principalTable: "Beneficiarios",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Pagos_CuentasAhorro_IdCuentaPago",
                        column: x => x.IdCuentaPago,
                        principalTable: "CuentasAhorro",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "Prestamos",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    MontoPrestamo = table.Column<decimal>(type: "decimal(18,2)", precision: 18, scale: 2, nullable: false),
                    DeudaRestante = table.Column<decimal>(type: "decimal(18,2)", precision: 18, scale: 2, nullable: false),
                    FechaInicio = table.Column<DateTime>(type: "datetime2", nullable: false),
                    IdProductoFinanciero = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Prestamos", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ProductosFinancieros",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    IdentificadorUnico = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    IdUsuario = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    NumeroProducto = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    FechaCreacion = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CuentaAhorroId = table.Column<int>(type: "int", nullable: false),
                    PrestamoId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProductosFinancieros", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ProductosFinancieros_CuentasAhorro_CuentaAhorroId",
                        column: x => x.CuentaAhorroId,
                        principalTable: "CuentasAhorro",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ProductosFinancieros_Prestamos_PrestamoId",
                        column: x => x.PrestamoId,
                        principalTable: "Prestamos",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "TarjetasCredito",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    IdentificadorUnico = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    NumeroTarjeta = table.Column<string>(type: "nvarchar(max)", nullable: false),
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
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "Transacciones",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    IdUsuario = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    TipoTransaccion = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Monto = table.Column<decimal>(type: "decimal(18,2)", precision: 18, scale: 2, nullable: false),
                    Fecha = table.Column<DateTime>(type: "datetime2", nullable: false),
                    IdCuentaOrigen = table.Column<int>(type: "int", nullable: true),
                    IdCuentaDestino = table.Column<int>(type: "int", nullable: true),
                    IdProductoFinanciero = table.Column<int>(type: "int", nullable: true)
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
                        name: "FK_Transacciones_ProductosFinancieros_IdProductoFinanciero",
                        column: x => x.IdProductoFinanciero,
                        principalTable: "ProductosFinancieros",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.SetNull);
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
                name: "IX_Beneficiarios_IdCuentaBeneficiario",
                table: "Beneficiarios",
                column: "IdCuentaBeneficiario");

            migrationBuilder.CreateIndex(
                name: "IX_CuentasAhorro_IdProductoFinanciero",
                table: "CuentasAhorro",
                column: "IdProductoFinanciero");

            migrationBuilder.CreateIndex(
                name: "IX_Pagos_IdBeneficiario",
                table: "Pagos",
                column: "IdBeneficiario");

            migrationBuilder.CreateIndex(
                name: "IX_Pagos_IdCuentaPago",
                table: "Pagos",
                column: "IdCuentaPago");

            migrationBuilder.CreateIndex(
                name: "IX_Pagos_IdProductoFinanciero",
                table: "Pagos",
                column: "IdProductoFinanciero");

            migrationBuilder.CreateIndex(
                name: "IX_Prestamos_IdProductoFinanciero",
                table: "Prestamos",
                column: "IdProductoFinanciero",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_ProductosFinancieros_CuentaAhorroId",
                table: "ProductosFinancieros",
                column: "CuentaAhorroId");

            migrationBuilder.CreateIndex(
                name: "IX_ProductosFinancieros_PrestamoId",
                table: "ProductosFinancieros",
                column: "PrestamoId");

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
                name: "IX_Transacciones_IdProductoFinanciero",
                table: "Transacciones",
                column: "IdProductoFinanciero");

            migrationBuilder.AddForeignKey(
                name: "FK_AvancesEfectivo_CuentasAhorro_IdCuentaDestino",
                table: "AvancesEfectivo",
                column: "IdCuentaDestino",
                principalTable: "CuentasAhorro",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_AvancesEfectivo_TarjetasCredito_IdTarjetaCredito",
                table: "AvancesEfectivo",
                column: "IdTarjetaCredito",
                principalTable: "TarjetasCredito",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Beneficiarios_CuentasAhorro_IdCuentaBeneficiario",
                table: "Beneficiarios",
                column: "IdCuentaBeneficiario",
                principalTable: "CuentasAhorro",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_CuentasAhorro_ProductosFinancieros_IdProductoFinanciero",
                table: "CuentasAhorro",
                column: "IdProductoFinanciero",
                principalTable: "ProductosFinancieros",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Pagos_ProductosFinancieros_IdProductoFinanciero",
                table: "Pagos",
                column: "IdProductoFinanciero",
                principalTable: "ProductosFinancieros",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Prestamos_ProductosFinancieros_IdProductoFinanciero",
                table: "Prestamos",
                column: "IdProductoFinanciero",
                principalTable: "ProductosFinancieros",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ProductosFinancieros_CuentasAhorro_CuentaAhorroId",
                table: "ProductosFinancieros");

            migrationBuilder.DropForeignKey(
                name: "FK_Prestamos_ProductosFinancieros_IdProductoFinanciero",
                table: "Prestamos");

            migrationBuilder.DropTable(
                name: "AvancesEfectivo");

            migrationBuilder.DropTable(
                name: "Pagos");

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

            migrationBuilder.DropTable(
                name: "Prestamos");
        }
    }
}
