

namespace InternetBanking.Core.Domain.Entities
{
    public class ProductoFinanciero
    {

        public int Id { get; set; } 
        public string IdentificadorUnico { get; set; }  
        public string? IdUsuario { get; set; }  
        public string NumeroProducto { get; set; }  
        public DateTime FechaCreacion { get; set; }

        public CuentasAhorro? CuentaAhorro { get; set; }
        public TarjetasCredito? TarjetaCredito { get; set; }
        public Prestamos? Prestamo { get; set; }

        public ICollection<CuentasAhorro> CuentasAhorro { get; set; }

    }
}
