using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InternetBanking.Core.Application.ViewModels.CuentasAhorro
{
    public class CuentasAhorroViewModel
    {
        public int Id { get; set; }
        public string NumeroCuenta { get; set; }
        public decimal Balance { get; set; }
        public bool EsPrincipal { get; set; }

        public string NumeroProducto { get; set; }

        public string IdentificadorUnico { get; set; }
    }
}
