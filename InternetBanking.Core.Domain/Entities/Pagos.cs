using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InternetBanking.Core.Domain.Entities
{
    public class Pagos
    {
        public Guid Id { get; set; }

        public string? IdUsuario { get; set; }

        public string TipoPago { get; set; }
        public decimal Monto { get; set; }
        public DateTime Fecha { get; set; }

        public Guid? IdCuentaPago { get; set; }
        public Guid? IdBeneficiario { get; set; }
        public Guid? IdProductoFinanciero { get; set; }
    }
}
