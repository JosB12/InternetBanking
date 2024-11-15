using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InternetBanking.Core.Application.ViewModels
{
    public class PagoPrestamoViewModel
    {
        [Required]
        [Display(Name = "Préstamo")]
        public int IdPrestamo { get; set; } // Relacionado con los préstamos del cliente

        [Required]
        [Display(Name = "Cuenta de Origen")]
        public int IdCuentaOrigen { get; set; } // Relacionado con las cuentas del cliente

        [Required]
        [Range(0.01, double.MaxValue, ErrorMessage = "El monto debe ser mayor a 0")]
        [Display(Name = "Monto a Pagar")]
        public decimal Monto { get; set; }
    }
}
