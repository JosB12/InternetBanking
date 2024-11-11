using InternetBanking.Core.Application.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InternetBanking.Core.Application.ViewModels.ProductosFinancieros
{
    public class ProductosFinancierosSaveViewModel
    {
        public TipoProducto TipoProducto { get; set; }
        public decimal MontoInicial { get; set; }
        public string UserId { get; set; }
    }
}
