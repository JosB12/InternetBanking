using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InternetBanking.Core.Application.ViewModels.CuentasAhorro
{
    public class SaveCuentasAhorroViewModel
    {
        [Required]
        [StringLength(20, MinimumLength = 10, ErrorMessage = "El número de cuenta debe tener entre 10 y 20 caracteres.")]
        public string NumeroCuenta { get; set; }

        [Required]
        [Range(0.01, double.MaxValue, ErrorMessage = "El balance debe ser mayor que 0.")]
        public decimal Balance { get; set; }

        [Required]
        public bool EsPrincipal { get; set; }

        // El producto financiero será seleccionado y está relacionado con la entidad de CuentasAhorro
        public int IdProductoFinanciero { get; set; }
    }
}
