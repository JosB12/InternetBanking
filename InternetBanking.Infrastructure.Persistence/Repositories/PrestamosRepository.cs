using InternetBanking.Core.Application.Interfaces.Repositories;
using InternetBanking.Core.Domain.Entities;
using InternetBanking.Infrastructure.Persistence.Contexts;
using InternetBanking.Infrastructure.Persistence.Repositories.Generic;
using Microsoft.EntityFrameworkCore;


namespace InternetBanking.Infrastructure.Persistence.Repositories
{
    public class PrestamosRepository : GenericRepository<Prestamos>, IPrestamosRepository
    {
        private readonly ApplicationContext _dbContext;

        public PrestamosRepository(ApplicationContext dbContext) : base(dbContext)
        {
            _dbContext = dbContext;
        }

        // Obtiene un préstamo con detalles asociados
        public async Task<Prestamos> GetWithDetailsByIdAsync(int id)
        {
            return await _dbContext.Prestamos
                .Include(p => p.ProductoFinanciero) // Relación con ProductoFinanciero
                .FirstOrDefaultAsync(p => p.Id == id);
        }

        public async Task<Prestamos> GetByProductoFinancieroIdAsync(int productoFinancieroId)
        {
            return await _dbContext.Prestamos
                .Where(p => p.IdProductoFinanciero == productoFinancieroId)
                .FirstOrDefaultAsync();
        }

        // Obtiene todos los préstamos asociados a un usuario
        public async Task<List<Prestamos>> GetByUserIdAsync(string userId)
        {
            return await _dbContext.Prestamos
                .Where(p => p.ProductoFinanciero.IdUsuario == userId)
                .Include(p => p.ProductoFinanciero) // Relación con ProductoFinanciero
                .ToListAsync();
        }
        public async Task UpdateAsync(Prestamos prestamo)
        {
            // Verificamos si el préstamo existe en la base de datos
            var existingPrestamo = await _dbContext.Prestamos
                .FirstOrDefaultAsync(p => p.Id == prestamo.Id);

            if (existingPrestamo != null)
            {
                // Actualizamos el monto del préstamo
                existingPrestamo.MontoPrestamo = prestamo.MontoPrestamo;

                // Guardamos los cambios en la base de datos
                await _dbContext.SaveChangesAsync();
            }
            else
            {
                throw new Exception("El préstamo no existe.");
            }
        }
    }
}
