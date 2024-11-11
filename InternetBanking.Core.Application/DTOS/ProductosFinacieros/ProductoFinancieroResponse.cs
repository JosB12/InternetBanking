using InternetBanking.Core.Application.Enums;
using InternetBanking.Core.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InternetBanking.Core.Application.DTOS.ProductosFinacieros
{
    public class ProductoFinancieroResponse
    {
                  
        public string IdentificadorUnico { get; set; }
        public TipoProducto TipoProducto { get; set; }      
        public decimal BalanceInicial { get; set; }    
        public decimal LimiteCredito { get; set; }     
        public decimal MontoPrestamo { get; set; }

        public bool HasError { get; set; }           
        public string Error { get; set; }            
        public string Message { get; set; }
    }
}
