using InternetBanking.Core.Application.Interfaces.Repositories;
using InternetBanking.Core.Application.Interfaces.Repositories.Pago;
using InternetBanking.Core.Domain.Entities;
using InternetBanking.Core.Domain.Enums;
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
    public class ProductosFinancierosRepository : GenericRepository<ProductosFinancieros>, IProductosFinancierosRepository
    {
        private readonly ApplicationContext _dbContext;

        public ProductosFinancierosRepository(ApplicationContext dbContext) : base(dbContext)
        {
            _dbContext = dbContext;
        }

        #region sector 1
        public async Task<ProductosFinancieros> GetByUserIdAndProductTypeAsync(string userId, TipoProducto tipoProducto)
        {
            return await _dbContext.ProductosFinancieros
                .FirstOrDefaultAsync(p => p.IdUsuario == userId && p.TipoProducto == tipoProducto);
        }

        public async Task<ProductosFinancieros> GetByIdentificadorUnicoAsync(string numeroProducto)
        {
            return await _dbContext.ProductosFinancieros
                .FirstOrDefaultAsync(p => p.NumeroProducto == numeroProducto);
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
        public async Task<bool> ExistsByIdentificadorUnicoAsync(string numeroProducto)
        {
            return await _dbContext.ProductosFinancieros
                .AnyAsync(p => p.NumeroProducto == numeroProducto);
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
        public async Task<int> ObtenerCantidadTotalProductosFinancierosAsync() 
        { 
            return await _dbContext.ProductosFinancieros.CountAsync(); 
        }
            
        public async Task<List<ProductosFinancieros>> GetByUserIdAsync(string userId)
        {
            return await _dbContext.ProductosFinancieros
                .Where(p => p.IdUsuario == userId)
                .Include(p => p.CuentaAhorro)
                .Include(p => p.TarjetaCredito)
                .Include(p => p.Prestamo)
                .ToListAsync();
        }
        #endregion

    }
}
