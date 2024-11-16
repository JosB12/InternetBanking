using InternetBanking.Core.Application.Interfaces.Repositories.Generic;
using InternetBanking.Core.Domain.Entities;


namespace InternetBanking.Core.Application.Interfaces.Repositories
{
    public interface ICuentasAhorroRepository : IGenericRepository<CuentasAhorro>
    {
        Task<CuentasAhorro> GetPrimaryAccountByUserIdAsync(string userId);
        Task<CuentasAhorro> GetSavingsAccountByUserIdAsync(string userId);
        Task<CuentasAhorro> GetWithDetailsByIdAsync(int id);
        Task<List<CuentasAhorro>> GetByUserIdAsync(string userId);
        Task<bool> CuentasAhorroUpdateAsync(CuentasAhorro cuenta);
        Task<CuentasAhorro> GetPrincipalAccountByUserIdAsync(string userId);
        Task<CuentasAhorro> GetByProductoIdAsync(int productoId);

        Task<CuentasAhorro> GetByIdentificadorUnicoAsync(string identificadorUnico);
        Task<CuentasAhorro> GetCuentaByNumeroCuentaAsync(string numeroCuenta);

    }
}
