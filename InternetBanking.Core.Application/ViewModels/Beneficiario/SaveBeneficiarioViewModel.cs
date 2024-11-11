using System.ComponentModel.DataAnnotations;

namespace InternetBanking.Core.Application.ViewModels.Beneficiario
{
    public class SaveBeneficiarioViewModel
    {
        [Required(ErrorMessage = "El número de cuenta es requerido.")]
        [Display(Name = "Número de Cuenta")]
        public string NumeroCuenta { get; set; }

    }
}
