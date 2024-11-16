using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InternetBanking.Core.Application.ViewModels.Pago
{
    public class SavePagoExpresoViewModel
    {
        public string NumeroCuenta { get; set; }
        public decimal Monto { get; set; }
        public int IdCuentaPago { get; set; }
        public string? IdentificadorUnico { get; set; }  
        public string? NombrePropietarioCuentaDestino { get; set; } 
    }
}
