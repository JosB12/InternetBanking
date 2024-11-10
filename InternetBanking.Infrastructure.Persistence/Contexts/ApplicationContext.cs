using Microsoft.EntityFrameworkCore;
using InternetBanking.Core.Domain.Entities;

namespace InternetBanking.Infrastructure.Persistence.Contexts
{
    public class ApplicationContext : DbContext
    {
        public ApplicationContext(DbContextOptions<ApplicationContext> options) : base(options) { }

        public DbSet<ProductosFinancieros> ProductosFinancieros { get; set; }
        public DbSet<CuentasAhorro> CuentasAhorro { get; set; }
        public DbSet<TarjetasCredito> TarjetasCredito { get; set; }
        public DbSet<Prestamos> Prestamos { get; set; }
        public DbSet<Beneficiarios> Beneficiarios { get; set; }
        public DbSet<Transacciones> Transacciones { get; set; }
        public DbSet<Pagos> Pagos { get; set; }
        public DbSet<AvancesEfectivo> AvancesEfectivo { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Relación AvancesEfectivo -> TarjetasCredito (N a 1)
            modelBuilder.Entity<AvancesEfectivo>()
                .HasOne(a => a.TarjetaCredito)
                .WithMany()
                .HasForeignKey(a => a.IdTarjetaCredito);

            // Relación AvancesEfectivo -> CuentasAhorro (N a 1)
            modelBuilder.Entity<AvancesEfectivo>()
                .HasOne(a => a.CuentaDestino)
                .WithMany()
                .HasForeignKey(a => a.IdCuentaDestino);

            // Relación Beneficiarios -> CuentasAhorro (N a 1)
            modelBuilder.Entity<Beneficiarios>()
                .HasOne(b => b.CuentaBeneficiario)
                .WithMany()
                .HasForeignKey(b => b.IdCuentaBeneficiario);

            // Relación CuentasAhorro -> ProductosFinancieros (1 a N)
            modelBuilder.Entity<CuentasAhorro>()
                .HasOne(c => c.ProductoFinanciero)
                .WithMany(p => p.CuentasAhorro)
                .HasForeignKey(c => c.IdProductoFinanciero);

            // Relación Pagos -> CuentasAhorro (N a 1, opcional)
            modelBuilder.Entity<Pagos>()
                .HasOne(p => p.CuentaPago)
                .WithMany()
                .HasForeignKey(p => p.IdCuentaPago)
                .IsRequired(false);

            // Relación Pagos -> Beneficiarios (N a 1, opcional)
            modelBuilder.Entity<Pagos>()
                .HasOne(p => p.Beneficiario)
                .WithMany()
                .HasForeignKey(p => p.IdBeneficiario)
                .IsRequired(false);

            // Relación Pagos -> ProductosFinancieros (N a 1, opcional)
            modelBuilder.Entity<Pagos>()
                .HasOne(p => p.ProductoFinanciero)
                .WithMany()
                .HasForeignKey(p => p.IdProductoFinanciero)
                .IsRequired(false);

            // Relación Prestamos -> ProductosFinancieros (1 a 1)
            modelBuilder.Entity<Prestamos>()
                .HasOne(p => p.ProductoFinanciero)
                .WithOne()
                .HasForeignKey<Prestamos>(p => p.IdProductoFinanciero);

            // Relación TarjetasCredito -> ProductosFinancieros (1 a 1)
            modelBuilder.Entity<TarjetasCredito>()
                .HasOne(t => t.ProductoFinanciero)
                .WithOne()
                .HasForeignKey<TarjetasCredito>(t => t.IdProductoFinanciero);

            // Relación Transacciones -> CuentasAhorro (origen) (N a 1, opcional)
            modelBuilder.Entity<Transacciones>()
                .HasOne(t => t.CuentaOrigen)
                .WithMany()
                .HasForeignKey(t => t.IdCuentaOrigen)
                .IsRequired(false);

            // Relación Transacciones -> CuentasAhorro (destino) (N a 1, opcional)
            modelBuilder.Entity<Transacciones>()
                .HasOne(t => t.CuentaDestino)
                .WithMany()
                .HasForeignKey(t => t.IdCuentaDestino)
                .IsRequired(false);

            // Relación Transacciones -> ProductosFinancieros (N a 1, opcional)
            modelBuilder.Entity<Transacciones>()
                .HasOne(t => t.ProductoFinanciero)
                .WithMany()
                .HasForeignKey(t => t.IdProductoFinanciero)
                .IsRequired(false);
        }

    }
}
