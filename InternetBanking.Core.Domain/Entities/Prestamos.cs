using System.ComponentModel.DataAnnotations;

namespace InternetBanking.Core.Domain.Entities
{
    public class Prestamos
    {
        public int Id { get; set; }
        public decimal MontoPrestamo { get; set; }
        public decimal DeudaRestante { get; set; }
        public DateTime FechaInicio { get; set; }

        [Required]
        public int IdProductoFinanciero { get; set; }
        public virtual ProductosFinancieros ProductoFinanciero { get; set; }
    }
}
