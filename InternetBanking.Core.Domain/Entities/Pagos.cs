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
        public int Id { get; set; }

        public string? IdUsuario { get; set; }

        public string TipoPago { get; set; }
        public decimal Monto { get; set; }
        public DateTime Fecha { get; set; }

        public int? IdCuentaPago { get; set; }
        public CuentasAhorro CuentaPago { get; set; }  // Propiedad de navegación

        public int? IdBeneficiario { get; set; }
        public Beneficiarios Beneficiario { get; set; }  // Propiedad de navegación

        public int? IdProductoFinanciero { get; set; }
        public ProductosFinancieros ProductoFinanciero { get; set; }


    }
}
