﻿using InternetBanking.Core.Application.Interfaces.Repositories.Generic;
using InternetBanking.Core.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InternetBanking.Core.Application.Interfaces.Repositories.Transaccion
{
    public interface ITransaccionesRepository : IGenericRepository<Transacciones>
    {
        Task<int> ObtenerCantidadTotalTransaccionesAsync();
        Task<int> ObtenerCantidadTransaccionesDelDiaAsync();
    }
}