using InternetBanking.Core.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using InternetBanking.Core.Application.Interfaces.Services.Generic;
using System.Threading.Tasks;
using InternetBanking.Core.Application.DTOS.General;
using InternetBanking.Core.Application.ViewModels;

namespace InternetBanking.Core.Application.Interfaces.Services
{
    public interface IAvancesEfectivoService
    {
        Task<Response> RealizarAvanceAsync(int tarjetaCreditoId, int cuentaAhorroId, decimal monto);
        Task<List<AvanceEfectivoViewModel>> GetAllAvancesAsync();
    }
}

