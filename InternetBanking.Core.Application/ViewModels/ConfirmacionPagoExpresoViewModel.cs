using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InternetBanking.Core.Application.ViewModels
{
    public class ConfirmacionPagoExpresoViewModel
    {
        public string NombreDestinatario { get; set; }
        public string ApellidoDestinatario { get; set; }
        public string NumeroCuentaDestino { get; set; }
        public decimal Monto { get; set; }
        public int IdCuentaOrigen { get; set; }
    }
}
