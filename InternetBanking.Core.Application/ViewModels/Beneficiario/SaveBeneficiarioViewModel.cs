using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InternetBanking.Core.Application.ViewModels.Beneficiario
{
    public class SaveBeneficiarioViewModel
    {
        [Required(ErrorMessage = "El número de cuenta es requerido.")]
        [Display(Name = "Número de Cuenta")]
        public string? NumeroCuenta { get; set; }

        public string? IdUsuario { get; set; }

        public int IdProductoFinanciero { get; set; }
    }
}
