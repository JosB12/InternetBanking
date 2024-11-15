using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InternetBanking.Core.Application.ViewModels
{
    public class PagoTarjetaCreditoViewModel
    {
        [Required]
        [Display(Name = "Tarjeta de Crédito")]
        public int IdTarjetaCredito { get; set; } // Relacionado con las tarjetas del cliente

        [Required]
        [Display(Name = "Cuenta de Origen")]
        public int IdCuentaOrigen { get; set; } // Relacionado con las cuentas del cliente

        [Required]
        [Range(0.01, double.MaxValue, ErrorMessage = "El monto debe ser mayor a 0")]
        [Display(Name = "Monto a Pagar")]
        public decimal Monto { get; set; }
    }
}
