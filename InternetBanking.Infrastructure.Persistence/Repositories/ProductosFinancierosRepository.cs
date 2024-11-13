using InternetBanking.Core.Application.Interfaces.Repositories;
using InternetBanking.Core.Domain.Entities;
using InternetBanking.Infrastructure.Persistence.Contexts;
using InternetBanking.Infrastructure.Persistence.Repositories.Generic;
using Microsoft.EntityFrameworkCore;


namespace InternetBanking.Infrastructure.Persistence.Repositories
{
    public class ProductosFinancierosRepository : GenericRepository<ProductosFinancieros>, IProductosFinancierosRepository
    {
        private readonly ApplicationContext _dbContext;

        public ProductosFinancierosRepository(ApplicationContext dbContext) : base(dbContext)
        {
            _dbContext = dbContext;
        }
        public async Task<ProductosFinancieros> GetByIdentificadorUnicoAsync(string identificadorUnico)
        {
            return await _dbContext.ProductosFinancieros
                .FirstOrDefaultAsync(p => p.IdentificadorUnico == identificadorUnico);
        }
        // Obtiene un producto financiero con todos los detalles de las relaciones asociadas
        public async Task<ProductosFinancieros> GetByIdAsync(int id)
        {
            return await _dbContext.ProductosFinancieros
                .Include(p => p.CuentaAhorro)
                .Include(p => p.TarjetaCredito)
                .Include(p => p.Prestamo)
                .FirstOrDefaultAsync(p => p.Id == id);
        }
        public async Task<bool> ExistsByIdentificadorUnicoAsync(string identificadorUnico)
        {
            return await _dbContext.ProductosFinancieros
                .AnyAsync(p => p.IdentificadorUnico == identificadorUnico);
        }

        // Método para agregar un nuevo producto financiero
        public override async Task<ProductosFinancieros> AddAsync(ProductosFinancieros entity)
        {
            await _dbContext.ProductosFinancieros.AddAsync(entity);
            await _dbContext.SaveChangesAsync();
            return entity;
        }
        public async Task<bool> DeleteAsync(ProductosFinancieros producto)
        {
            _dbContext.ProductosFinancieros.Remove(producto);
            var result = await _dbContext.SaveChangesAsync();
            return result > 0; // Retorna true si se eliminaron filas, false si no
        }

   
        // Consulta para obtener todos los productos financieros asociados a un usuario
        public async Task<List<ProductosFinancieros>> GetByUserIdAsync(string userId)
        {
            return await _dbContext.ProductosFinancieros
                .Where(p => p.IdUsuario == userId)
                .Include(p => p.CuentaAhorro)
                .Include(p => p.TarjetaCredito)
                .Include(p => p.Prestamo)
                .ToListAsync();
        }
    }
}
