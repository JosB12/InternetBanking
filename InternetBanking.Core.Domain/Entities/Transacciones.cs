using System.ComponentModel.DataAnnotations;
using InternetBanking.Core.Domain.Enums;

namespace InternetBanking.Core.Domain.Entities
{
    public class Transacciones
    {

        public int Id { get; set; }
        [Required]
        public string IdUsuario { get; set; } // Identificador de usuario, sin referencia a ApplicationUser
        [Required]
        public TipoTransaccion TipoTransaccion { get; set; }
        public decimal Monto { get; set; }
        public DateTime Fecha { get; set; }

        public int? IdCuentaOrigen { get; set; }
        public virtual CuentasAhorro CuentaOrigen { get; set; }

        public int? IdCuentaDestino { get; set; }
        public virtual CuentasAhorro CuentaDestino { get; set; }

        public int? IdProductoFinanciero { get; set; }
        public virtual ProductosFinancieros ProductoFinanciero { get; set; }
    }
}
