using InternetBanking.Core.Application.Interfaces.Services.Generic;
using InternetBanking.Core.Application.ViewModels.Transferencia;
using InternetBanking.Core.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InternetBanking.Core.Application.Interfaces.Services.Transaccion
{
    public interface ITransaccionesService : IGenericService<TransaccionesViewModel, TransaccionesViewModel, Transacciones>
    {
        Task<bool> RealizarTransferenciaAsync(TransaccionesViewModel model);
    }
}
