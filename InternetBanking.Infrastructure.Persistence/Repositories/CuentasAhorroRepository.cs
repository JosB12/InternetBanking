using InternetBanking.Core.Application.Interfaces.Repositories;
using InternetBanking.Core.Domain.Entities;
using InternetBanking.Infrastructure.Persistence.Contexts;
using InternetBanking.Infrastructure.Persistence.Repositories.Generic;
using Microsoft.EntityFrameworkCore;

namespace InternetBanking.Infrastructure.Persistence.Repositories
{
    public class CuentasAhorroRepository : GenericRepository<CuentasAhorro>, ICuentasAhorroRepository
    {
        private readonly ApplicationContext _dbContext;

        public CuentasAhorroRepository(ApplicationContext dbContext) : base(dbContext)
        {
            _dbContext = dbContext;
        }
        public async Task<CuentasAhorro> GetPrimaryAccountByUserIdAsync(string userId)
        {
            return await _dbContext.CuentasAhorro
                .FirstOrDefaultAsync(c => c.IdentificadorUnico == userId && c.EsPrincipal);
        }
        public async Task<CuentasAhorro> GetSavingsAccountByUserIdAsync(string userId)
        {
            // Busca en ProductosFinancieros usando el userId
            var productoFinanciero = await _dbContext.ProductosFinancieros
                                                   .FirstOrDefaultAsync(pf => pf.IdUsuario == userId);

            if (productoFinanciero == null)
            {
                // Si no se encuentra un producto financiero asociado, retorna null
                return null;
            }

            // Usa el ID de ProductosFinancieros para buscar en CuentasAhorro
            return await _dbContext.CuentasAhorro
                                 .FirstOrDefaultAsync(ca => ca.IdProductoFinanciero == productoFinanciero.Id);
        }
        public async Task<bool> CuentasAhorroUpdateAsync(CuentasAhorro cuenta)
        {
            _dbContext.CuentasAhorro.Update(cuenta);
            return await _dbContext.SaveChangesAsync() > 0;
        }

    }
}
