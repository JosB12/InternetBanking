using InternetBanking.Core.Application.Interfaces.Repositories.Generic;
using InternetBanking.Core.Domain.Entities;


namespace InternetBanking.Core.Application.Interfaces.Repositories
{
    public interface ICuentasAhorroRepository : IGenericRepository<CuentasAhorro>
    {
        Task<(bool exists, CuentasAhorro cuenta, string idUsuario)> ValidateAccountAsync(string numeroCuenta);
        Task<CuentasAhorro> GetAccountByIdAndNumeroCuentaAsync(int idCuenta, string numeroCuenta);
    }
}
