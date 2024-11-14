using InternetBanking.Core.Application.Interfaces.Repositories.Generic;
using InternetBanking.Core.Domain.Entities;
using InternetBanking.Core.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InternetBanking.Core.Application.Interfaces.Repositories
{
    public interface IProductosFinancierosRepository : IGenericRepository<ProductosFinancieros>
    {
        Task<int> ObtenerCantidadTotalProductosFinancierosAsync();
        Task<ProductosFinancieros> GetByIdentificadorUnicoAsync(string identificadorUnico);
        Task<ProductosFinancieros> GetByIdAsync(int id);
        Task<bool> ExistsByIdentificadorUnicoAsync(string identificadorUnico);
        Task<bool> DeleteAsync(ProductosFinancieros producto);
        Task<List<ProductosFinancieros>> GetByUserIdAsync(string userId);
        Task<ProductosFinancieros> AddAsync(ProductosFinancieros entity);
        Task<ProductosFinancieros> GetByUserIdAndProductTypeAsync(string userId, TipoProducto tipoProducto);

    }
}
