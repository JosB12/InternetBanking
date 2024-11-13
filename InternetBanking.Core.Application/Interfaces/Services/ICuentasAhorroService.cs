using InternetBanking.Core.Application.Interfaces.Services.Generic;
using InternetBanking.Core.Application.ViewModels.Beneficiario;
using InternetBanking.Core.Application.ViewModels.CuentasAhorro;
using InternetBanking.Core.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InternetBanking.Core.Application.Interfaces.Services
{
    public interface ICuentasAhorroService : IGenericService<SaveCuentasAhorroViewModel, CuentasAhorroViewModel, CuentasAhorro>
    {
        //Task<CuentasAhorroViewModel> GetAccountByNumeroCuentaAsync(string numeroCuenta);
    }
}
