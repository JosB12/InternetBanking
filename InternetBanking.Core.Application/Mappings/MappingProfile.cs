using AutoMapper;
using InternetBanking.Core.Application.ViewModels.Productos;
using InternetBanking.Core.Domain.Entities;

namespace InternetBanking.Core.Application.Mappings
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<ProductoSaveViewModel, ProductosFinancieros>();
            CreateMap<ProductoSaveViewModel, CuentasAhorro>();
            CreateMap<ProductoSaveViewModel, TarjetasCredito>();
            CreateMap<ProductoSaveViewModel, Prestamos>();
            // ...otros mapeos...
        }
    }
}
