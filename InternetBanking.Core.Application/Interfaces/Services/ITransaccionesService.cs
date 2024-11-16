using System.Threading.Tasks;
using InternetBanking.Core.Application.ViewModels.Transacciones;
using InternetBanking.Core.Domain.Entities;
using InternetBanking.Core.Application.Interfaces.Services.Generic;
using System.Collections.Generic;
using InternetBanking.Core.Application.ViewModels;

namespace InternetBanking.Core.Application.Interfaces.Services
{
    public interface ITransaccionesService : IGenericService<TransaccionesSaveViewModel, TransaccionesViewModel, Transacciones>
    {
        Task<bool> TransferirFondos(TransferenciaViewModel transferenciaVm);
        Task<List<MostrarCuentaAhorroViewModel>> GetCuentasByUserIdAsync(string userId);
    }
}
