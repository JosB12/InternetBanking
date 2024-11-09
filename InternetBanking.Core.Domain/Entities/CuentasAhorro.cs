using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InternetBanking.Core.Domain.Entities
{
    public class CuentasAhorro
    {
        public Guid Id { get; set; }

        public string NumeroCuenta { get; set; }

        public decimal Balance { get; set; }

        public bool EsPrincipal { get; set; }

        public Guid IdProductoFinanciero { get; set; }

        public ProductosFinancieros ProductoFinanciero { get; set; }
    }
}
