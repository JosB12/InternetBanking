using InternetBanking.Core.Domain.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InternetBanking.Core.Application.ViewModels
{
    public class SaveProductosFinancierosViewModel
    {
        public int? Id { get; set; }

        [Required]
        public string IdUsuario { get; set; }

        [Required]
        public TipoProducto TipoProducto { get; set; }

        [Required]
        public string IdentificadorUnico { get; set; }

        // Propiedades específicas por tipo de producto
        public decimal? LimiteCredito { get; set; } // Solo para tarjetas de crédito
        public decimal? MontoPrestamo { get; set; } // Solo para préstamos
        public bool? EsPrincipal { get; set; } // Solo para cuentas de ahorro
        public decimal? Balance { get; set; } // Solo para cuentas de ahorro (Balance inicial)
    }
}
