using InternetBanking.Core.Application.Interfaces.Repositories.Generic;
using InternetBanking.Core.Domain.Entities;

namespace InternetBanking.Core.Application.Interfaces.Repositories
{
    public interface IProductosFinancierosRepository : IGenericRepository<ProductosFinancieros>
    {
        Task<ProductosFinancieros> GetByIdentificadorUnicoAsync(string identificadorUnico);
    }
}
