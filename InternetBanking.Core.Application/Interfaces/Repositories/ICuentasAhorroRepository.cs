using InternetBanking.Core.Application.Interfaces.Repositories.Generic;
using InternetBanking.Core.Domain.Entities;


namespace InternetBanking.Core.Application.Interfaces.Repositories
{
    public interface ICuentasAhorroRepository : IGenericRepository<CuentasAhorro>
    {
        Task<CuentasAhorro> GetPrimaryAccountByUserIdAsync(string userId);
        Task<CuentasAhorro> GetSavingsAccountByUserIdAsync(string userId);
        Task<bool> CuentasAhorroUpdateAsync(CuentasAhorro cuenta);
    }
}
