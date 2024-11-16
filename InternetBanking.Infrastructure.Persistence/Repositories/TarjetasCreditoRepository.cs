using InternetBanking.Core.Application.Interfaces.Repositories;
using InternetBanking.Core.Domain.Entities;
using InternetBanking.Infrastructure.Persistence.Contexts;
using InternetBanking.Infrastructure.Persistence.Repositories.Generic;
using Microsoft.EntityFrameworkCore;

namespace InternetBanking.Infrastructure.Persistence.Repositories
{
    public class TarjetasCreditoRepository : GenericRepository<TarjetasCredito>, ITarjetasCreditoRepository
    {
        private readonly ApplicationContext _dbContext;

        public TarjetasCreditoRepository(ApplicationContext dbContext) : base(dbContext)
        {
            _dbContext = dbContext;
        }

        // Obtiene una tarjeta de crédito con detalles asociados
        public async Task<TarjetasCredito> GetWithDetailsByIdAsync(int id)
        {
            return await _dbContext.TarjetasCredito
                .Include(t => t.ProductoFinanciero) // Relación con ProductoFinanciero
                .FirstOrDefaultAsync(t => t.Id == id);
        }

        public async Task UpdateDeudaAsync(int tarjetaCreditoId, decimal montoConInteres)
        {
            var tarjetaCredito = await _dbContext.TarjetasCredito
                .FirstOrDefaultAsync(tc => tc.Id == tarjetaCreditoId);

            if (tarjetaCredito != null)
            {
                tarjetaCredito.DeudaActual += montoConInteres;
                _dbContext.TarjetasCredito.Update(tarjetaCredito);
                await _dbContext.SaveChangesAsync();
            }
            else
            {
                throw new Exception("Tarjeta de crédito no encontrada.");
            }
        }

        // Obtiene todas las tarjetas de crédito asociadas a un usuario
        public async Task<List<TarjetasCredito>> GetByUserIdAsync(string userId)
        {
            return await _dbContext.TarjetasCredito
                .Where(t => t.ProductoFinanciero.IdUsuario == userId)
                .Include(t => t.ProductoFinanciero) // Relación con ProductoFinanciero
                .ToListAsync();
        }

        public async Task<TarjetasCredito> GetByProductoFinancieroIdAsync(int productoFinancieroId)
        {
            return await _dbContext.TarjetasCredito
                .FirstOrDefaultAsync(t => t.IdProductoFinanciero == productoFinancieroId);
        }

        public async Task UpdateAsync(TarjetasCredito tarjetaCredito)
        {
            // Primero verificamos si la tarjeta ya existe en la base de datos
            var existingTarjeta = await _dbContext.TarjetasCredito
                .FirstOrDefaultAsync(t => t.Id == tarjetaCredito.Id);

            if (existingTarjeta != null)
            {
                // Actualizamos el límite de crédito
                existingTarjeta.LimiteCredito = tarjetaCredito.LimiteCredito;

                // Guardamos los cambios en la base de datos
                await _dbContext.SaveChangesAsync();
            }
            else
            {
                throw new Exception("La tarjeta de crédito no existe.");
            }
        }

        public async Task<TarjetasCredito> GetByIdentificadorUnicoAsync(string identificadorUnico)
        {
            return await _dbContext.TarjetasCredito
                .FirstOrDefaultAsync(t => t.NumeroTarjeta == identificadorUnico);
        }
    }
}
