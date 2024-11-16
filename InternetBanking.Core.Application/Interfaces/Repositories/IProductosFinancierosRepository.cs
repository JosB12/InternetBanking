﻿using InternetBanking.Core.Application.Interfaces.Repositories.Generic;
using InternetBanking.Core.Domain.Entities;
using InternetBanking.Core.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InternetBanking.Core.Application.Interfaces.Repositories
{
    public interface IProductosFinancierosRepository : IGenericRepository<ProductosFinancieros>
    {
        #region sector 1
        Task<int> ObtenerCantidadTotalProductosFinancierosAsync();
        Task<ProductosFinancieros> GetByIdentificadorUnicoAsync(string numeroProducto);
        Task<ProductosFinancieros> GetByIdAsync(int id);
        Task<bool> ExistsByIdentificadorUnicoAsync(string numeroProducto);
        Task<bool> DeleteAsync(ProductosFinancieros producto);
        Task<List<ProductosFinancieros>> GetByUserIdAsync(string userId);
        Task<ProductosFinancieros> AddAsync(ProductosFinancieros entity);
        Task<ProductosFinancieros> GetByUserIdAndProductTypeAsync(string userId, TipoProducto tipoProducto);
<<<<<<< HEAD
        Task<List<ProductosFinancieros>> GetByClienteIdAsync(int clienteId);
=======
        #endregion


>>>>>>> origin/client-cash-advances

    }
}
