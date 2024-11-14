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
        public async Task UpdateCuentaexistenteAsync(CuentasAhorro entity)
        {
            // Obtenemos el id de la entidad (siempre que tenga la propiedad Id)
            var existingEntity = await _dbContext.CuentasAhorro.FindAsync(entity.Id);
            if (existingEntity != null)
            {
                _dbContext.Entry(existingEntity).CurrentValues.SetValues(entity);
                await _dbContext.SaveChangesAsync();
            }
        }
        public async Task<CuentasAhorro> GetByProductoIdAsync(int productoId)
        {
            return await _dbContext.CuentasAhorro
                .FirstOrDefaultAsync(c => c.IdProductoFinanciero == productoId);
        }
        public async Task<CuentasAhorro> GetPrincipalAccountByProductIdAsync(int productoId)
        {
            // Devuelve la cuenta de ahorro principal asociada al producto financiero
            return await _dbContext.CuentasAhorro
                .Where(c => c.EsPrincipal && c.IdProductoFinanciero == productoId)
                .FirstOrDefaultAsync();
        }

        public async Task<CuentasAhorro> GetPrincipalAccountByUserIdAsync(string userId)
        {
            return await _dbContext.CuentasAhorro
                .Where(c => c.ProductoFinanciero.IdUsuario == userId && c.EsPrincipal)
                .FirstOrDefaultAsync();
        }

        //public async Task<CuentasAhorro> GetPrimaryAccountByUserIdAsync(string userId)
        //{
        //    return await _dbContext.CuentasAhorro
        //        .FirstOrDefaultAsync(c => c.ProductoFinanciero.IdUsuario == userId && c.EsPrincipal);
        //}


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
        // Obtiene una cuenta de ahorro con detalles asociados
        public async Task<CuentasAhorro> GetWithDetailsByIdAsync(int id)
        {
            return await _dbContext.CuentasAhorro
                .Include(c => c.ProductoFinanciero) // Relación con ProductoFinanciero
                .Include(c => c.TransaccionesOrigen)
                .Include(c => c.TransaccionesDestino)
                .Include(c => c.Pagos)
                .Include(c => c.AvancesEfectivo)
                .FirstOrDefaultAsync(c => c.Id == id);
        }
        public async Task<List<CuentasAhorro>> GetByUserIdAsync(string userId)
        {
            return await _dbContext.CuentasAhorro
                .Where(c => c.ProductoFinanciero.IdUsuario == userId)
                .Include(c => c.ProductoFinanciero) // Relación con ProductoFinanciero
                .ToListAsync();
        }
        public async Task<bool> CuentasAhorroUpdateAsync(CuentasAhorro cuenta)
        {
            _dbContext.CuentasAhorro.Update(cuenta);
            return await _dbContext.SaveChangesAsync() > 0;
        }

    }
}
