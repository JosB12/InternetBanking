using InternetBanking.Core.Application.Interfaces.Repositories.Pago;
using InternetBanking.Core.Application.Interfaces.Repositories.Transaccion;
using InternetBanking.Core.Application.Interfaces.Repositories;
using InternetBanking.Core.Application.Interfaces.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using InternetBanking.Core.Application.Interfaces.Services.User;
using InternetBanking.Core.Application.Interfaces.Services.Account;

namespace InternetBanking.Core.Application.Services
{
    public class DashboardService : IDashboardService
    {
        private readonly ITransaccionesRepository _transaccionesRepository;
        private readonly IPagosRepository _pagosRepository;
        private readonly IProductosFinancierosRepository _productosFinancierosRepository;
        private readonly IAccountService _accountService;
        public DashboardService(
            ITransaccionesRepository transaccionesRepository,
            IPagosRepository pagosRepository,
            IProductosFinancierosRepository productosFinancierosRepository,
            IAccountService accountService )
        {
            _transaccionesRepository = transaccionesRepository;
            _pagosRepository = pagosRepository;
            _productosFinancierosRepository = productosFinancierosRepository;
            _accountService = accountService;
            
        }

        public async Task<int> ObtenerCantidadTotalTransaccionesAsync()
        {
            return await _transaccionesRepository.ObtenerCantidadTotalTransaccionesAsync();
        }

        public async Task<int> ObtenerCantidadTransaccionesDelDiaAsync()
        {
            return await _transaccionesRepository.ObtenerCantidadTransaccionesDelDiaAsync();
        }

        public async Task<int> ObtenerCantidadTotalPagosAsync()
        {
            return await _pagosRepository.ObtenerCantidadTotalPagosAsync();
        }

        public async Task<int> ObtenerCantidadPagosDelDiaAsync()
        {
            return await _pagosRepository.ObtenerCantidadPagosDelDiaAsync();
        }

        public async Task<int> ObtenerCantidadClientesActivosAsync()
        {
            return await _accountService.ObtenerTotalClientesActivosAsync();
        }

        public async Task<int> ObtenerCantidadClientesInactivosAsync()
        {
            return await _accountService.ObtenerTotalClientesInactivosAsync();
        }

        public async Task<int> ObtenerCantidadTotalProductosFinancierosAsync()
        {
            return await _productosFinancierosRepository.ObtenerCantidadTotalProductosFinancierosAsync();
        }
    }
}
