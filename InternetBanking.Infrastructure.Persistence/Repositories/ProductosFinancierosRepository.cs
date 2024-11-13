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
    }
}
