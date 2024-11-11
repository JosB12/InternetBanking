using System.ComponentModel.DataAnnotations;

namespace InternetBanking.Core.Domain.Entities
{
    public class TarjetasCredito
    {
        public int Id { get; set; }
        [Required]
        public string IdentificadorUnico { get; set; }
        [Required]
        public string NumeroTarjeta { get; set; } 
        public decimal LimiteCredito { get; set; }
        public decimal DeudaActual { get; set; }

        [Required]
        public int IdProductoFinanciero { get; set; }
        public virtual ProductosFinancieros ProductoFinanciero { get; set; }

        public virtual ICollection<AvancesEfectivo> AvancesEfectivo { get; set; }
    }
}
