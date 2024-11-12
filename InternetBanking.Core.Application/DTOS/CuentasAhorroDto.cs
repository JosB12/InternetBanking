using InternetBanking.Core.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InternetBanking.Core.Application.DTOS
{
    public class CuentasAhorroDto
    {
        public string Id { get; set; }
        public string NumeroCuenta { get; set; }
        public TipoProducto TipoProducto { get; set; }
    }
}
