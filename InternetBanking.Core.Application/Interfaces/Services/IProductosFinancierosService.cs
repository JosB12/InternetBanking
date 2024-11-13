using InternetBanking.Core.Application.Interfaces.Repositories;
using InternetBanking.Core.Application.Interfaces.Services.Generic;
using InternetBanking.Core.Application.Services;
using InternetBanking.Core.Application.ViewModels;
using InternetBanking.Core.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InternetBanking.Core.Application.Interfaces.Services
{
    public interface IProductosFinancierosService : IGenericService<SaveProductosFinancierosViewModel, ProductosFinancierosViewModel, ProductosFinancieros>
    {
        Task<SaveProductosFinancierosViewModel> AddProductoAsync(SaveProductosFinancierosViewModel model);
        Task<bool> DeleteProductoAsync(string identificadorUnico);
        Task<List<ProductosFinancierosViewModel>> GetProductosByUserIdAsync(string userId);
    }
}
