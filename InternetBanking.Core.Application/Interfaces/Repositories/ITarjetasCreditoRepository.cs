using InternetBanking.Core.Application.Interfaces.Repositories.Generic;
using InternetBanking.Core.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InternetBanking.Core.Application.Interfaces.Repositories
{
    public interface ITarjetasCreditoRepository : IGenericRepository<TarjetasCredito>
    {
        Task<TarjetasCredito> GetWithDetailsByIdAsync(int id);
        Task<List<TarjetasCredito>> GetByUserIdAsync(string userId);
        Task UpdateAsync(TarjetasCredito tarjetaCredito);
        Task<TarjetasCredito> GetByProductoFinancieroIdAsync(int productoFinancieroId);

        Task<TarjetasCredito> GetByIdentificadorUnicoAsync(string identificadorUnico);

    }
}
