using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InternetBanking.Core.Application.ViewModels
{
    public class MostrarTarjetaCreditoViewModel
    {
        public string NumeroTarjeta { get; set; }
        public decimal LimiteCredito { get; set; }
        public decimal DeudaActual { get; set; }
    }
}
