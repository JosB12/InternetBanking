using System.ComponentModel.DataAnnotations;

namespace InternetBanking.Core.Domain.Entities
{
    public class AvancesEfectivo
    {
        public int Id { get; set; }
        public decimal Monto { get; set; }
        public decimal Interes { get; set; }
        public DateTime FechaAvance { get; set; }

        [Required]
        public int IdTarjetaCredito { get; set; }
        public virtual TarjetasCredito TarjetaCredito { get; set; }

        [Required]
        public int IdCuentaDestino { get; set; }
        public virtual CuentasAhorro CuentaDestino { get; set; }
    }
}
