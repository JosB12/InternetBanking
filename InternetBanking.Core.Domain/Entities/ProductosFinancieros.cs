using InternetBanking.Core.Domain.Enums;

using System.ComponentModel.DataAnnotations;

namespace InternetBanking.Core.Domain.Entities
{
    public class ProductosFinancieros
    {

        public int Id { get; set; }
        public string IdentificadorUnico { get; set; } // 9 dígitos únicos en el sistema
        [Required]
        public string IdUsuario { get; set; }
        public string NumeroProducto { get; set; } // Hasta 20 caracteres
        public DateTime FechaCreacion { get; set; }
        public TipoProducto TipoProducto { get; set; }

        // Navegación
        public virtual CuentasAhorro CuentaAhorro { get; set; }
        public virtual TarjetasCredito TarjetaCredito { get; set; }
        public virtual Prestamos Prestamo { get; set; }

    }
}
