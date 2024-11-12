using InternetBanking.Core.Application.DTOS;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InternetBanking.Core.Application.ViewModels.Transferencia
{
    public class TransaccionesViewModel
    {

        [Required]
        public int CuentaOrigenId { get; set; }

        [Required]
        public int CuentaDestinoId { get; set; }

        [Required]
        [Range(0.01, double.MaxValue, ErrorMessage = "El monto debe ser mayor a cero")]
        public decimal Monto { get; set; }
        public List<CuentasAhorroDto> CuentasUsuario { get; set; }
        public string UsuarioId { get; set; }


    }
}
