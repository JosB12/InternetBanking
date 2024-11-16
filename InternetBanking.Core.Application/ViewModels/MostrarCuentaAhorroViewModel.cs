using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InternetBanking.Core.Application.ViewModels
{
    public class MostrarCuentaAhorroViewModel
    {
        public int Id { get; set; }
        public string NumeroCuenta { get; set; }
        public decimal Balance { get; set; }
    }
}
