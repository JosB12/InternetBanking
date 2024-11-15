using InternetBanking.Core.Application.Interfaces.Repositories.Generic;
using InternetBanking.Core.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InternetBanking.Core.Application.Interfaces.Repositories
{
    public interface IAvancesEfectivoRepository : IGenericRepository<AvancesEfectivo>
    {
        Task<TarjetasCredito> GetTarjetaCreditoByIdAsync(string tarjetaCreditoId);
        Task<CuentasAhorro> GetCuentaAhorroByIdAsync(string cuentaAhorroId);
        Task<bool> UpdateCuentaAhorroBalanceAsync(string cuentaAhorroId, decimal nuevoBalance);
        Task<bool> UpdateTarjetaCreditoDeudaAsync(string tarjetaCreditoId, decimal montoAvance);
    }
}
