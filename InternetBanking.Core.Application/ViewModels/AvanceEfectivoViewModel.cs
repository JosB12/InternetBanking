using System.ComponentModel.DataAnnotations;

namespace InternetBanking.Core.Application.ViewModels
{
    public class AvanceEfectivoViewModel
    {
        [Required(ErrorMessage = "El campo 'Tarjeta de Crédito' es obligatorio.")]
        public int TarjetaCreditoId { get; set; } // ID de la tarjeta seleccionada

        [Required(ErrorMessage = "El campo 'Cuenta de Ahorro' es obligatorio.")]
        public int CuentaAhorroId { get; set; }  // ID de la cuenta seleccionada

        [Required]
        [Range(1, double.MaxValue, ErrorMessage = "El monto debe ser positivo")]
        public decimal Monto { get; set; }

  


    }

}
