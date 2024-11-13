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
        public DbSet<Pagos> Pagos { get; set; }
        public DbSet<Transacciones> Transacciones { get; set; }
        public DbSet<AvancesEfectivo> AvancesEfectivo { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            //FLUENT API
            #region tables
            modelBuilder.Entity<ProductosFinancieros>().ToTable("ProductosFinancieros");
            modelBuilder.Entity<CuentasAhorro>().ToTable("CuentasAhorro");
            modelBuilder.Entity<TarjetasCredito>().ToTable("TarjetasCredito");
            modelBuilder.Entity<Prestamos>().ToTable("Prestamos");
            modelBuilder.Entity<Beneficiarios>().ToTable("Beneficiarios");
            modelBuilder.Entity<Pagos>().ToTable("Pagos");
            modelBuilder.Entity<Transacciones>().ToTable("Transacciones");
            modelBuilder.Entity<AvancesEfectivo>().ToTable("AvancesEfectivo");
            #endregion

            #region "primary keys"
            modelBuilder.Entity<ProductosFinancieros>().HasKey(pf => pf.Id);
            modelBuilder.Entity<CuentasAhorro>().HasKey(ca => ca.Id);
            modelBuilder.Entity<TarjetasCredito>().HasKey(tc => tc.Id);
            modelBuilder.Entity<Prestamos>().HasKey(p => p.Id);
            modelBuilder.Entity<Beneficiarios>().HasKey(b => b.Id);
            modelBuilder.Entity<Pagos>().HasKey(p => p.Id);
            modelBuilder.Entity<Transacciones>().HasKey(t => t.Id);
            modelBuilder.Entity<AvancesEfectivo>().HasKey(ae => ae.Id);
            #endregion

            #region "Relationships"
            // ProductosFinancieros relationships
            modelBuilder.Entity<ProductosFinancieros>()
                .HasOne(pf => pf.CuentaAhorro)
                .WithOne(ca => ca.ProductoFinanciero)
                .HasForeignKey<CuentasAhorro>(ca => ca.IdProductoFinanciero);

            modelBuilder.Entity<ProductosFinancieros>()
                .HasOne(pf => pf.TarjetaCredito)
                .WithOne(tc => tc.ProductoFinanciero)
                .HasForeignKey<TarjetasCredito>(tc => tc.IdProductoFinanciero);

            modelBuilder.Entity<ProductosFinancieros>()
                .HasOne(pf => pf.Prestamo)
                .WithOne(p => p.ProductoFinanciero)
                .HasForeignKey<Prestamos>(p => p.IdProductoFinanciero);

            // CuentasAhorro relationships
            modelBuilder.Entity<CuentasAhorro>()
                .HasMany(ca => ca.TransaccionesOrigen)
                .WithOne(t => t.CuentaOrigen)
                .HasForeignKey(t => t.IdCuentaOrigen)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<CuentasAhorro>()
                .HasMany(ca => ca.TransaccionesDestino)
                .WithOne(t => t.CuentaDestino)
                .HasForeignKey(t => t.IdCuentaDestino)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<CuentasAhorro>()
                .HasMany(ca => ca.Pagos)
                .WithOne(p => p.CuentaPago)
                .HasForeignKey(p => p.IdCuentaPago)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<CuentasAhorro>()
                .HasMany(ca => ca.AvancesEfectivo)
                .WithOne(ae => ae.CuentaDestino)
                .HasForeignKey(ae => ae.IdCuentaDestino)
                .OnDelete(DeleteBehavior.Restrict);

            // TarjetasCredito relationships
            modelBuilder.Entity<TarjetasCredito>()
                .HasMany(tc => tc.AvancesEfectivo)
                .WithOne(ae => ae.TarjetaCredito)
                .HasForeignKey(ae => ae.IdTarjetaCredito)
                .OnDelete(DeleteBehavior.Restrict);

            // Beneficiarios relationships
            modelBuilder.Entity<Beneficiarios>()
                .HasMany(b => b.Pagos)
                .WithOne(p => p.Beneficiario)
                .HasForeignKey(p => p.IdBeneficiario)
                .OnDelete(DeleteBehavior.Restrict);
            #endregion

            #region "Property configurations"
            // ProductosFinancieros
            modelBuilder.Entity<ProductosFinancieros>()
                .Property(pf => pf.IdentificadorUnico)
                .IsRequired()
                .HasMaxLength(9);

            modelBuilder.Entity<ProductosFinancieros>()
                .Property(pf => pf.IdUsuario)
                .IsRequired();

            modelBuilder.Entity<ProductosFinancieros>()
                .Property(pf => pf.NumeroProducto)
                .HasMaxLength(20);

            // CuentasAhorro
            modelBuilder.Entity<CuentasAhorro>()
                .Property(ca => ca.IdentificadorUnico)
                .IsRequired()
                .HasMaxLength(9);

            modelBuilder.Entity<CuentasAhorro>()
                .Property(ca => ca.NumeroCuenta)
                .IsRequired()
                .HasMaxLength(20);

            modelBuilder.Entity<CuentasAhorro>()
                .Property(ca => ca.Balance)
                .HasPrecision(18, 2);

            // TarjetasCredito
            modelBuilder.Entity<TarjetasCredito>()
                .Property(tc => tc.IdentificadorUnico)
                .IsRequired()
                .HasMaxLength(9);

            modelBuilder.Entity<TarjetasCredito>()
                .Property(tc => tc.NumeroTarjeta)
                .IsRequired()
                .HasMaxLength(16);

            modelBuilder.Entity<TarjetasCredito>()
                .Property(tc => tc.LimiteCredito)
                .HasPrecision(18, 2);

            modelBuilder.Entity<TarjetasCredito>()
                .Property(tc => tc.DeudaActual)
                .HasPrecision(18, 2);

            // Prestamos
            modelBuilder.Entity<Prestamos>()
                .Property(p => p.MontoPrestamo)
                .HasPrecision(18, 2);

            modelBuilder.Entity<Prestamos>()
                .Property(p => p.DeudaRestante)
                .HasPrecision(18, 2);

            // Beneficiarios
            modelBuilder.Entity<Beneficiarios>()
                .Property(b => b.IdUsuario)
                .IsRequired();

            modelBuilder.Entity<Beneficiarios>()
                .Property(b => b.Nombre)
                .IsRequired()
                .HasMaxLength(100);

            modelBuilder.Entity<Beneficiarios>()
                .Property(b => b.Apellido)
                .IsRequired()
                .HasMaxLength(100);

            modelBuilder.Entity<Beneficiarios>()
                .Property(b => b.NumeroCuenta)
                .IsRequired()
                .HasMaxLength(20);

            // Pagos
            modelBuilder.Entity<Pagos>()
                .Property(p => p.IdUsuario)
                .IsRequired();

            modelBuilder.Entity<Pagos>()
                .Property(p => p.Monto)
                .HasPrecision(18, 2);

            // Transacciones
            modelBuilder.Entity<Transacciones>()
                .Property(t => t.IdUsuario)
                .IsRequired();

            modelBuilder.Entity<Transacciones>()
                .Property(t => t.Monto)
                .HasPrecision(18, 2);

            // AvancesEfectivo
            modelBuilder.Entity<AvancesEfectivo>()
                .Property(ae => ae.Monto)
                .HasPrecision(18, 2);

            modelBuilder.Entity<AvancesEfectivo>()
                .Property(ae => ae.Interes)
                .HasPrecision(18, 2);
            #endregion
        }
    }
}