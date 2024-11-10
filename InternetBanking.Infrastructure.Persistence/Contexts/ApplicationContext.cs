using Microsoft.EntityFrameworkCore;
using InternetBanking.Core.Domain.Entities;

namespace InternetBanking.Infrastructure.Persistence.Contexts
{
    public class ApplicationContext : DbContext
    {
        public ApplicationContext(DbContextOptions<ApplicationContext> options) : base(options) { }

        #region tables
        public DbSet<ProductosFinancieros> ProductosFinancieros { get; set; }
        public DbSet<CuentasAhorro> CuentasAhorro { get; set; }
        public DbSet<TarjetasCredito> TarjetasCredito { get; set; }
        public DbSet<Prestamos> Prestamos { get; set; }
        public DbSet<Beneficiarios> Beneficiarios { get; set; }
        public DbSet<Transacciones> Transacciones { get; set; }
        public DbSet<Pagos> Pagos { get; set; }
        public DbSet<AvancesEfectivo> AvancesEfectivo { get; set; }
        #endregion

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {

            #region relationships
            // Relación AvancesEfectivo -> TarjetasCredito (N a 1)
            modelBuilder.Entity<AvancesEfectivo>()
                .HasOne(a => a.TarjetaCredito)
                .WithMany()
                .HasForeignKey(a => a.IdTarjetaCredito)
                .OnDelete(DeleteBehavior.Restrict); // Se puede ajustar a NoAction, Restrict, etc.

            // Relación AvancesEfectivo -> CuentasAhorro (N a 1)
            modelBuilder.Entity<AvancesEfectivo>()
                .HasOne(a => a.CuentaDestino)
                .WithMany()
                .HasForeignKey(a => a.IdCuentaDestino)
                .OnDelete(DeleteBehavior.Cascade); // Se puede ajustar a NoAction, Restrict, etc.

            // Relación Beneficiarios -> CuentasAhorro (N a 1)
            modelBuilder.Entity<Beneficiarios>()
                .HasOne(b => b.CuentaBeneficiario)
                .WithMany()
                .HasForeignKey(b => b.IdCuentaBeneficiario)
                .OnDelete(DeleteBehavior.Restrict); // Se puede ajustar a NoAction, Restrict, etc.

            // Relación CuentasAhorro -> ProductosFinancieros (1 a N)
            modelBuilder.Entity<CuentasAhorro>()
                .HasOne(c => c.ProductoFinanciero)
                .WithMany(p => p.CuentasAhorro)
                .HasForeignKey(c => c.IdProductoFinanciero)
                .OnDelete(DeleteBehavior.Restrict); // Se puede ajustar a NoAction, Restrict, etc.

            // Relación Pagos -> CuentasAhorro (N a 1, opcional)
            modelBuilder.Entity<Pagos>()
                .HasOne(p => p.CuentaPago)
                .WithMany()
                .HasForeignKey(p => p.IdCuentaPago)
                .IsRequired(false)
                .OnDelete(DeleteBehavior.NoAction); // Opcional, cambia el comportamiento

            // Relación Pagos -> Beneficiarios (N a 1, opcional)
            modelBuilder.Entity<Pagos>()
                .HasOne(p => p.Beneficiario)
                .WithMany()
                .HasForeignKey(p => p.IdBeneficiario)
                .IsRequired(false)
                .OnDelete(DeleteBehavior.NoAction); // Opcional, cambia el comportamiento

            // Relación Pagos -> ProductosFinancieros (N a 1, opcional)
            modelBuilder.Entity<Pagos>()
                .HasOne(p => p.ProductoFinanciero)
                .WithMany()
                .HasForeignKey(p => p.IdProductoFinanciero)
                .IsRequired(false)
                .OnDelete(DeleteBehavior.NoAction); // Opcional, cambia el comportamiento

            // Relación Prestamos -> ProductosFinancieros (1 a 1)
            modelBuilder.Entity<Prestamos>()
                .HasOne(p => p.ProductoFinanciero)
                .WithOne()
                .HasForeignKey<Prestamos>(p => p.IdProductoFinanciero)
                .OnDelete(DeleteBehavior.Restrict); // Se puede ajustar a NoAction, Restrict, etc.

            // Relación TarjetasCredito -> ProductosFinancieros (1 a 1)
            modelBuilder.Entity<TarjetasCredito>()
                .HasOne(t => t.ProductoFinanciero)
                .WithOne()
                .HasForeignKey<TarjetasCredito>(t => t.IdProductoFinanciero)
                .OnDelete(DeleteBehavior.Cascade); // Se puede ajustar a NoAction, Restrict, etc.
            modelBuilder.Entity<ProductosFinancieros>()
                .HasOne(p => p.TarjetaCredito)
                .WithOne(t => t.ProductoFinanciero)  // Ajusta la relación si es necesario
                .OnDelete(DeleteBehavior.NoAction);
            // Relación Transacciones -> CuentasAhorro (origen) (N a 1, opcional)
            modelBuilder.Entity<Transacciones>()
                .HasOne(t => t.CuentaOrigen)
                .WithMany()
                .HasForeignKey(t => t.IdCuentaOrigen)
                .IsRequired(false)
                .OnDelete(DeleteBehavior.Restrict); // Cambiar a NoAction o Restrict si es necesario

            // Relación Transacciones -> CuentasAhorro (destino) (N a 1, opcional)
            modelBuilder.Entity<Transacciones>()
                .HasOne(t => t.CuentaDestino)
                .WithMany()
                .HasForeignKey(t => t.IdCuentaDestino)
                .IsRequired(false)
                .OnDelete(DeleteBehavior.Restrict); // Cambiar a NoAction o Restrict si es necesario

            // Relación Transacciones -> ProductosFinancieros (N a 1, opcional)
            modelBuilder.Entity<Transacciones>()
                .HasOne(t => t.ProductoFinanciero)
                .WithMany()
                .HasForeignKey(t => t.IdProductoFinanciero)
                .IsRequired(false)
                .OnDelete(DeleteBehavior.SetNull); // Cambiar a NoAction o Restrict si es necesario
            #endregion


            #region Properties configuration
            modelBuilder.Entity<AvancesEfectivo>()
                .Property(a => a.Interes)
                .HasPrecision(18, 2);

            modelBuilder.Entity<AvancesEfectivo>()
                .Property(a => a.Monto)
                .HasPrecision(18, 2);

            modelBuilder.Entity<CuentasAhorro>()
                .Property(c => c.Balance)
                .HasPrecision(18, 2);

            modelBuilder.Entity<Pagos>()
                .Property(p => p.Monto)
                .HasPrecision(18, 2);

            modelBuilder.Entity<Prestamos>()
                .Property(p => p.DeudaRestante)
                .HasPrecision(18, 2);

            modelBuilder.Entity<Prestamos>()
                .Property(p => p.MontoPrestamo)
                .HasPrecision(18, 2);

            modelBuilder.Entity<TarjetasCredito>()
                .Property(t => t.DeudaActual)
                .HasPrecision(18, 2);

            modelBuilder.Entity<TarjetasCredito>()
                .Property(t => t.LimiteCredito)
                .HasPrecision(18, 2);

            modelBuilder.Entity<Transacciones>()
                .Property(t => t.Monto)
                .HasPrecision(18, 2);
            #endregion
        }

    }
}
