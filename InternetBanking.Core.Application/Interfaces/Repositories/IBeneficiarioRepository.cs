using InternetBanking.Core.Application.Interfaces.Repositories.Generic;
using InternetBanking.Core.Domain.Entities;
using System.Linq.Expressions;


namespace InternetBanking.Core.Application.Interfaces.Repositories
{
    public interface IBeneficiarioRepository : IGenericRepository<Beneficiarios>
    {
        Task<bool> AnyAsync(Expression<Func<Beneficiarios, bool>> predicate);

    }
}
