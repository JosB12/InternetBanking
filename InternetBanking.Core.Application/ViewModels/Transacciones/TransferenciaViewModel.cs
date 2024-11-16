using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace InternetBanking.Core.Application.ViewModels.Transacciones
{
    public class TransferenciaViewModel
    {
        [Required]
        public string IdUsuario { get; set; }

        [Required]
        public int IdCuentaOrigen { get; set; }

        [Required]
        public int IdCuentaDestino { get; set; }

        [Required]
        [Range(0.01, double.MaxValue, ErrorMessage = "El monto debe ser mayor a cero.")]
        public decimal Monto { get; set; }

        public List<MostrarCuentaAhorroViewModel>? Cuentas { get; set; }
    }
}
