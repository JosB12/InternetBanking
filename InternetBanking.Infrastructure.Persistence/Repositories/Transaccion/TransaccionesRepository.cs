using InternetBanking.Core.Application.Interfaces.Repositories.Transaccion;
using InternetBanking.Core.Domain.Entities;
using InternetBanking.Infrastructure.Persistence.Contexts;
using InternetBanking.Infrastructure.Persistence.Repositories.Generic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InternetBanking.Infrastructure.Persistence.Repositories.Transaccion
{
    public class TransaccionesRepository : GenericRepository<Transacciones>, ITransaccionesRepository
    {
        private readonly ApplicationContext _dbContext;

        public TransaccionesRepository(ApplicationContext dbContext) : base(dbContext)
        {
            _dbContext = dbContext;
        }
    }
}
