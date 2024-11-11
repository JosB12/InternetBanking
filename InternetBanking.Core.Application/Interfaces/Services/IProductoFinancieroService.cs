using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using InternetBanking.Core.Application.DTOS.ProductosFinacieros;

namespace InternetBanking.Core.Application.Interfaces.Services
{
    public interface IProductoFinancieroService
    {
        Task<ProductoFinancieroResponse> CrearCuentaAhorroAsync(decimal balanceInicial, string userId);

    }
}
