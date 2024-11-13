using System.ComponentModel.DataAnnotations;

namespace InternetBanking.Core.Domain.Entities
{
    public class Beneficiarios
    {
        public int Id { get; set; }
        [Required]
        public string IdUsuario { get; set; }
        [Required]
        public int IdCuentaBeneficiario { get; set; }
        public virtual CuentasAhorro CuentaBeneficiario { get; set; }
        [Required]
        public string Nombre { get; set; }
        [Required]
        public string Apellido { get; set; }
        [Required]
        public string NumeroCuenta { get; set; }

        public virtual ICollection<Pagos> Pagos { get; set; }
    }
}
