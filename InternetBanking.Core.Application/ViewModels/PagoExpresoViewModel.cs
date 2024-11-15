using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InternetBanking.Core.Application.ViewModels
{
    public class PagoExpresoViewModel
    {
        [Required]
        [Display(Name = "Número de Cuenta a Pagar")]
        public string NumeroCuentaDestino { get; set; }

        [Required]
        [Display(Name = "Cuenta de Origen")]
        public int IdCuentaOrigen { get; set; } // Relacionado con las cuentas del cliente

        [Required]
        [Range(0.01, double.MaxValue, ErrorMessage = "El monto debe ser mayor a 0")]
        [Display(Name = "Monto a Pagar")]
        public decimal Monto { get; set; }
    }
}
