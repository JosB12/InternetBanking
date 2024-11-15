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
    public class CuentasAhorroService : ICuentasAhorroService
    {
        private readonly ICuentasAhorroRepository _cuentasAhorroRepository;

        public CuentasAhorroService(ICuentasAhorroRepository cuentasAhorroRepository)
        {
            _cuentasAhorroRepository = cuentasAhorroRepository;
        }

        // Método para obtener las cuentas de ahorro del usuario por su ID
        public async Task<List<CuentasAhorro>> GetByUserIdAsync(string userId)
        {
            return await _cuentasAhorroRepository.GetByUserIdAsync(userId);
        }
    }
}
