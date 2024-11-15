using InternetBanking.Core.Application.Interfaces.Services.Generic;
using InternetBanking.Core.Application.ViewModels;
using InternetBanking.Core.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InternetBanking.Core.Application.Interfaces.Services
{
    public interface IDashboardService
    {
        Task<int> ObtenerCantidadTotalTransaccionesAsync();
        Task<int> ObtenerCantidadTransaccionesDelDiaAsync();
        Task<int> ObtenerCantidadTotalPagosAsync();
        Task<int> ObtenerCantidadPagosDelDiaAsync();
        Task<int> ObtenerCantidadClientesActivosAsync();
        Task<int> ObtenerCantidadClientesInactivosAsync();
        Task<int> ObtenerCantidadTotalProductosFinancierosAsync();
    }
}
