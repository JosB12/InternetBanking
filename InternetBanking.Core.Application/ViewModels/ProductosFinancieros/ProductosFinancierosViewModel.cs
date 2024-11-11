using InternetBanking.Core.Application.Enums;
using InternetBanking.Core.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InternetBanking.Core.Application.ViewModels.ProductosFinancieros
{
    public class ProductosFinancierosViewModel
    {
        public string IdentificadorUnico { get; set; }
        public TipoProducto TipoProducto { get; set; }
        public decimal Balance { get; set; }
        public string UserId { get; set; }
    }
}
