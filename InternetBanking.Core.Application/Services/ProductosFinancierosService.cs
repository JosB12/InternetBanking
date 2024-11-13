using InternetBanking.Core.Application.Interfaces.Repositories;
using InternetBanking.Core.Application.Interfaces.Services;
using InternetBanking.Core.Application.ViewModels.Productos;
using System.Threading.Tasks;

namespace InternetBanking.Core.Application.Services
{
    public class ProductosFinancierosService : IProductosFinancierosService
    {
        // ...código existente...

        public async Task<ProductoViewModel> AddProductosFinancieros(ProductoSaveViewModel saveViewModel)
        {
            // ...implementación...
        }

        public async Task DeleteProductosFinancieros(int id)
        {
            // ...implementación...
        }

        // ...otros métodos según sea necesario...
    }
}
