using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InternetBanking.Core.Domain.Entities
{
    public class Beneficiarios
    {
        [Key]
        public Guid Id { get; set; }

        [Required]
        public string? IdUsuario { get; set; }

        [Required]
        public Guid IdCuentaBeneficiario { get; set; }

        public CuentasAhorro CuentaBeneficiario { get; set; }

        public string Nombre { get; set; }

        public string Apellido { get; set; }


        public string NumeroCuenta { get; set; }
    }
}
