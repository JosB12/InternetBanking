using InternetBanking.Core.Application.ViewModels.Pago;
using InternetBanking.Core.Domain.Entities;
using InternetBanking.Core.Application.Interfaces.Services.Generic;

namespace InternetBanking.Core.Application.Interfaces.Services
{
    public interface IPagoService : IGenericService<SavePagoViewModel, PagoViewModel, Pagos>
    {
        Task<string> RealizarPagoExpreso(SavePagoExpresoViewModel model);
        Task<string> RealizarPagoTarjetaCredito(SavePagoViewModel model);
        Task<IEnumerable<CuentasAhorro>> GetCuentasByUserIdAsync(string userId);
        Task<IEnumerable<TarjetasCredito>> GetTarjetasByUserIdAsync(string userId);
        Task<CuentasAhorro> GetCuentaByIdentificadorUnicoAsync(string identificadorUnico);
    }

    
}
