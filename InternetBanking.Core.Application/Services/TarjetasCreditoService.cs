using InternetBanking.Core.Application.Interfaces.Repositories;
using InternetBanking.Core.Application.Interfaces.Services;
using InternetBanking.Core.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InternetBanking.Core.Application.Services
{
    public class TarjetasCreditoService : ITarjetasCreditoService
    {
        private readonly ITarjetasCreditoRepository _tarjetasCreditoRepository;

        public TarjetasCreditoService(ITarjetasCreditoRepository tarjetasCreditoRepository)
        {
            _tarjetasCreditoRepository = tarjetasCreditoRepository;
        }

        // Método para obtener las tarjetas de crédito del usuario por su ID
        public async Task<List<TarjetasCredito>> GetByUserIdAsync(string userId)
        {
            return await _tarjetasCreditoRepository.GetByUserIdAsync(userId);
        }
    }
}
