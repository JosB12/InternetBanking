using System.ComponentModel.DataAnnotations;
using InternetBanking.Core.Domain.Enums;

namespace InternetBanking.Core.Domain.Entities
{
    public class Pagos
    {
        public int Id { get; set; }
        [Required]
        public string IdUsuario { get; set; } // Identificador de usuario, sin referencia a ApplicationUser
        [Required]
        public TipoPago TipoPago { get; set; }
        public decimal Monto { get; set; }
        public DateTime Fecha { get; set; }

        public int? IdCuentaPago { get; set; }
        public virtual CuentasAhorro CuentaPago { get; set; }

        public int? IdCuentaDestino { get; set; }
        public virtual CuentasAhorro CuentaDestino { get; set; }

        public int? IdBeneficiario { get; set; }
        public virtual Beneficiarios Beneficiario { get; set; }

        public int? IdProductoFinanciero { get; set; }
       
    }
}
