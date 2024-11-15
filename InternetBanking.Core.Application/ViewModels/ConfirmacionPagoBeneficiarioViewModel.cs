using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InternetBanking.Core.Application.ViewModels
{
    public class ConfirmacionPagoBeneficiarioViewModel
    {
        public string NombreBeneficiario { get; set; }
        public string ApellidoBeneficiario { get; set; }
        public string NumeroCuentaBeneficiario { get; set; }
        public decimal Monto { get; set; }
        public int IdCuentaOrigen { get; set; }
    }
}
