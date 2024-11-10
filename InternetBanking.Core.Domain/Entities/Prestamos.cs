using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InternetBanking.Core.Domain.Entities
{
    public class Prestamos
    {
        
        public Guid Id { get; set; }

        public decimal MontoPrestamo { get; set; }
        public decimal DeudaRestante { get; set; }
        public DateTime FechaInicio { get; set; }

        public int IdProductoFinanciero { get; set; }

        public ProductosFinancieros ProductoFinanciero { get; set; }
    }
}
