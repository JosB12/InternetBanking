using InternetBanking.Core.Application.Interfaces.Repositories.Generic;
using InternetBanking.Core.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace InternetBanking.Core.Application.Interfaces.Repositories
{
    public interface IBeneficiarioRepository : IGenericRepository<Beneficiarios>
    {
        Task<bool> AnyAsync(Expression<Func<Beneficiarios, bool>> predicate);

    }
}
