using InternetBanking.Core.Application.Interfaces.Repositories;
using InternetBanking.Core.Domain.Entities;
using InternetBanking.Infrastructure.Persistence.Contexts;
using InternetBanking.Infrastructure.Persistence.Repositories.Generic;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;


namespace InternetBanking.Infrastructure.Persistence.Repositories
{
    public class BeneficiarioRepository : GenericRepository<Beneficiarios>, IBeneficiarioRepository
    {
        private readonly ApplicationContext _dbContext;

        public BeneficiarioRepository(ApplicationContext dbContext) : base(dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<bool> AnyAsync(Expression<Func<Beneficiarios, bool>> predicate)
        {
            return await _dbContext.Beneficiarios.AnyAsync(predicate);
        }
    }
}
