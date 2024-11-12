using InternetBanking.Core.Application.Interfaces.Repositories.Pago;
using InternetBanking.Core.Application.Interfaces.Repositories.Transaccion;
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
    public class PagoRepository : GenericRepository<Pagos>, IPagosRepository
    {
        private readonly ApplicationContext _dbContext;

        public PagoRepository(ApplicationContext dbContext) : base(dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<int> ObtenerCantidadTotalPagosAsync()
        {
            return await _dbContext.Pagos.CountAsync();
        }

        public async Task<int> ObtenerCantidadPagosDelDiaAsync()
        {
            return await _dbContext.Pagos
                .Where(p => p.Fecha.Date == DateTime.Today)
                .CountAsync();
        }
    }
}
