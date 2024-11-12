using InternetBanking.Core.Application.Interfaces.Repositories;
using InternetBanking.Core.Application.Interfaces.Repositories.Pago;
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
    public class ProductosFinancierosRepository : GenericRepository<ProductosFinancieros>, IProductosFinancierosRepository
    {
        private readonly ApplicationContext _dbContext;

        public ProductosFinancierosRepository(ApplicationContext dbContext) : base(dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<int> ObtenerCantidadTotalProductosFinancierosAsync()
        {
            return await _dbContext.ProductosFinancieros.CountAsync();
        }
    }
}
