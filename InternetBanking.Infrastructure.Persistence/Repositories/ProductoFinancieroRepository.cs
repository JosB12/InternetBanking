using InternetBanking.Core.Application.Interfaces.Repositories.Generic;
using InternetBanking.Core.Domain.Entities;
using InternetBanking.Infrastructure.Persistence.Contexts;
using Microsoft.EntityFrameworkCore;

using System.Linq.Expressions;


namespace InternetBanking.Infrastructure.Persistence.Repositories.Generic
{
    public class ProductoFinancieroRepository : GenericRepository<ProductoFinanciero>, IProductoFinancieroRepository
    {
        private readonly ApplicationContext _dbContext;

        public ProductoFinancieroRepository(ApplicationContext dbContext) : base(dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<List<ProductoFinanciero>> GetByClienteIdAsync(int clienteId)
        {
            return await _dbContext.ProductosFinancieros
                                   .Where(pf => pf.IdUsuario == clienteId.ToString())  // Convertir clienteId a string
                                   .ToListAsync();
        }

    }
}
