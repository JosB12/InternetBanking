using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InternetBanking.Core.Application.ViewModels
{
    public class MostrarProductoViewModel
    {
        public string TipoProducto { get; set; } // "CuentaAhorro", "TarjetaCredito", "Prestamo"
        public string IdentificadorUnico { get; set; }
        public string NumeroProducto { get; set; }

        // Propiedades específicas según el tipo de producto
        public MostrarCuentaAhorroViewModel CuentaAhorro { get; set; }
        public MostrarTarjetaCreditoViewModel TarjetaCredito { get; set; }
        public MostrarPrestamoViewModel Prestamo { get; set; }
    }
}
