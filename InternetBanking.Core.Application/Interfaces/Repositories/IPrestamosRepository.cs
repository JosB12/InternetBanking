using InternetBanking.Core.Application.Interfaces.Repositories.Generic;
using InternetBanking.Core.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InternetBanking.Core.Application.Interfaces.Repositories
{
    public interface IPrestamosRepository : IGenericRepository<Prestamos>
    {
        Task<Prestamos> GetWithDetailsByIdAsync(int id);
        Task<List<Prestamos>> GetByUserIdAsync(string userId);
        Task UpdateAsync(Prestamos prestamo);
        Task<Prestamos> GetByProductoFinancieroIdAsync(int idProductoFinanciero);
    }
}
