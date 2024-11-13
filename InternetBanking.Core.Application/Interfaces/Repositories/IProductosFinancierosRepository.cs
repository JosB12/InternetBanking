using InternetBanking.Core.Application.Interfaces.Repositories.Generic;
using InternetBanking.Core.Domain.Entities;

namespace InternetBanking.Core.Application.Interfaces.Repositories
{
    public interface IProductosFinancierosRepository : IGenericRepository<ProductosFinancieros>
    {
        Task<ProductosFinancieros> GetByIdentificadorUnicoAsync(string identificadorUnico);
        Task<ProductosFinancieros> GetByIdAsync(int id);
        Task<bool> ExistsByIdentificadorUnicoAsync(string identificadorUnico);
        Task<bool> DeleteAsync(ProductosFinancieros producto);
        Task<List<ProductosFinancieros>> GetByUserIdAsync(string userId);
        Task<ProductosFinancieros> AddAsync(ProductosFinancieros entity);

    }
}
