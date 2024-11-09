using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InternetBanking.Core.Domain.Entities
{
    public class ProductosFinancieros
    {
        
        public Guid Id { get; set; }

        public string TipoProducto { get; set; }

        public string IdUsuario { get; set; }

        public string NumeroProducto { get; set; } // Número único de 9 dígitos

        public DateTime FechaCreacion { get; set; }

        public CuentasAhorro CuentaAhorro { get; set; }
        public TarjetasCredito TarjetaCredito { get; set; }
        public Prestamos Prestamo { get; set; }
    }
}
