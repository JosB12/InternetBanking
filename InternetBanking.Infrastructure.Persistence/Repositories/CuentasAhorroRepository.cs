using InternetBanking.Core.Application.Interfaces.Repositories;
using InternetBanking.Core.Domain.Entities;
using InternetBanking.Infrastructure.Persistence.Contexts;
using InternetBanking.Infrastructure.Persistence.Repositories.Generic;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InternetBanking.Infrastructure.Persistence.Repositories
{
    public class CuentasAhorroRepository : GenericRepository<CuentasAhorro>, ICuentasAhorroRepository
    {
        private readonly ApplicationContext _dbContext;

        public CuentasAhorroRepository(ApplicationContext dbContext) : base(dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<CuentasAhorro> GetAccountByNumeroCuentaAsync(string numeroCuenta)
        {
            if (string.IsNullOrEmpty(numeroCuenta))
            {
                throw new ArgumentException("El número de cuenta no puede estar vacío", nameof(numeroCuenta));
            }

            var cuenta = await _dbContext.CuentasAhorro
                .FirstOrDefaultAsync(c => c.NumeroCuenta == numeroCuenta);

            if (cuenta == null)
            {
                throw new KeyNotFoundException("Cuenta no encontrada para el número de cuenta proporcionado.");
            }

            return cuenta;
        }

    }
}
