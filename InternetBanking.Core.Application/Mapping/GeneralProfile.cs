using AutoMapper;
using InternetBanking.Core.Application.DTOS.Account.Authentication;
using InternetBanking.Core.Application.DTOS.Account.Edit;
using InternetBanking.Core.Application.DTOS.Account.Get;
using InternetBanking.Core.Application.DTOS.Account.Register;
using InternetBanking.Core.Application.Enums;
using InternetBanking.Core.Application.ViewModels.User;
using InternetBanking.Core.Application.ViewModels;
using InternetBanking.Core.Domain.Entities;
using InternetBanking.Core.Application.ViewModels.Transacciones;
using InternetBanking.Core.Application.ViewModels.Pago;

namespace InternetBanking.Core.Application.Mapping
{
    public class GeneralProfile : Profile
    {
        public GeneralProfile()
        {
            #region UserProfile
            CreateMap<AuthenticationRequest, LogginViewModel>()
                .ForMember(x => x.HasError, opt => opt.Ignore())
                .ForMember(x => x.Error, opt => opt.Ignore())
                .ReverseMap();

            CreateMap<RegisterRequest, SaveUserViewModel>()
                 .ForMember(x => x.HasError, opt => opt.Ignore())
                 .ForMember(x => x.Error, opt => opt.Ignore())
                 .ForMember(dest => dest.TipoUsuario, opt => opt.MapFrom(src => src.TipoUsuario))
                 .ForMember(dest => dest.MontoInicial, opt => opt.MapFrom(src => src.MontoInicial))
                 .ReverseMap();

            CreateMap<UserDTO, UserListViewModel>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
                 .ForMember(dest => dest.Nombre, opt => opt.MapFrom(src => src.Nombre))
                 .ForMember(dest => dest.Apellido, opt => opt.MapFrom(src => src.Apellido))
                 .ForMember(dest => dest.Correo, opt => opt.MapFrom(src => src.Email))
                 .ForMember(dest => dest.TipoUsuario, opt => opt.MapFrom(src => src.TipoUsuario))
                 .ForMember(dest => dest.EstaActivo, opt => opt.MapFrom(src => src.EstaActivo));

            #endregion

            #region Productos
             CreateMap<ProductosFinancieros, ProductosFinancierosViewModel>()
                .ForMember(dest => dest.CuentaAhorro, opt => opt.MapFrom(src => src.CuentaAhorro))
                .ForMember(dest => dest.TarjetaCredito, opt => opt.MapFrom(src => src.TarjetaCredito))
                .ForMember(dest => dest.Prestamo, opt => opt.MapFrom(src => src.Prestamo))
                .ReverseMap();

            CreateMap<CuentasAhorro, MostrarCuentaAhorroViewModel>().ReverseMap();
            CreateMap<TarjetasCredito, MostrarTarjetaCreditoViewModel>().ReverseMap();
            CreateMap<Prestamos, MostrarPrestamoViewModel>().ReverseMap();

            CreateMap<SaveProductosFinancierosViewModel, ProductosFinancieros>().ReverseMap();
            #endregion


            #region Transacciones
             CreateMap<Transacciones, TransaccionesViewModel>().ReverseMap();
            CreateMap<Transacciones, TransaccionesSaveViewModel>().ReverseMap();
            CreateMap<Transacciones, TransferenciaViewModel>().ReverseMap();
            #endregion

            #region EditUser
            CreateMap<EditUserDTO, EditProfileViewModel>()
            .ReverseMap();
            #endregion

            #region Pagos
            CreateMap<Pagos, PagoExpresoViewModel>().ReverseMap();
            CreateMap<Pagos, PagoTarjetaCreditoViewModel>().ReverseMap();
            CreateMap<Pagos, SavePagoViewModel>().ReverseMap();
            CreateMap<Pagos, PagoViewModel>().ReverseMap();
            #endregion
        }
    }
}
