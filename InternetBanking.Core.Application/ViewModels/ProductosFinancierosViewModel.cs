using InternetBanking.Core.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InternetBanking.Core.Application.ViewModels
{
    public class ProductosFinancierosViewModel
    {
        public int Id { get; set; }
        public string IdentificadorUnico { get; set; }
        public string NumeroProducto { get; set; }
        public TipoProducto TipoProducto { get; set; }
        public DateTime FechaCreacion { get; set; }

        // Propiedades específicas por tipo de producto
        public decimal? LimiteCredito { get; set; } // Solo para tarjetas de crédito
        public decimal? MontoPrestamo { get; set; } // Solo para préstamos
        public decimal? DeudaRestante { get; set; } // Solo para préstamos

        // Propiedades específicas por tipo de producto
        public MostrarCuentaAhorroViewModel CuentaAhorro { get; set; } // Solo para cuentas de ahorro
        public MostrarTarjetaCreditoViewModel TarjetaCredito { get; set; } // Solo para tarjetas de crédito
        public MostrarPrestamoViewModel Prestamo { get; set; } // Solo para préstamos



    }

}
