using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InternetBanking.Core.Domain.Entities
{
    public class Transacciones
    {
        
        public int Id { get; set; }

        public string? IdUsuario { get; set; }

        public string TipoTransaccion { get; set; }
        public decimal Monto { get; set; }
        public DateTime Fecha { get; set; }

        public int? IdCuentaOrigen { get; set; }
        public CuentasAhorro CuentaOrigen { get; set; }  // Propiedad de navegación

        public int? IdCuentaDestino { get; set; }
        public CuentasAhorro CuentaDestino { get; set; }  // Propiedad de navegación

        public int? IdProductoFinanciero { get; set; }
        public ProductosFinancieros ProductoFinanciero { get; set; }
    }
}
