using AutoMapper;
using InternetBanking.Core.Application.DTOS.Account.Authentication;
using InternetBanking.Core.Application.DTOS.Account.Register;
using InternetBanking.Core.Application.ViewModels.Beneficiario;
using InternetBanking.Core.Application.ViewModels.CuentasAhorro;
using InternetBanking.Core.Application.ViewModels.User;
using InternetBanking.Core.Domain.Entities;


namespace InternetBanking.Core.Application.Mapping
{
    internal class GeneralProfile : Profile
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
                .ReverseMap();

            #endregion
            #region CuentaAhorro
            CreateMap<SaveCuentasAhorroViewModel, CuentasAhorro>()
                .ForMember(dest => dest.Balance, opt => opt.MapFrom(src => src.Balance))
                .ForMember(dest => dest.EsPrincipal, opt => opt.MapFrom(src => src.EsPrincipal));
            // .ReverseMap();


            CreateMap<CuentasAhorro, CuentasAhorroViewModel>()
               .ForMember(dest => dest.NumeroProducto, opt => opt.MapFrom(src => src.ProductoFinanciero.NumeroProducto))  // Mapeo de NumeroProducto
               .ForMember(dest => dest.IdentificadorUnico, opt => opt.MapFrom(src => src.ProductoFinanciero.IdentificadorUnico));  // Mapeo de IdentificadorUnico
                                                                                                                                   // .ReverseMap();
            #endregion

            #region Beneficiario
            CreateMap<SaveBeneficiarioViewModel, Beneficiarios>()
               .ForMember(dest => dest.NumeroCuenta, opt => opt.MapFrom(src => src.NumeroCuenta));

            CreateMap<Beneficiarios, BeneficiarioViewModel>()
                           .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
                           .ForMember(dest => dest.Nombre, opt => opt.MapFrom(src => src.Nombre))
                           .ForMember(dest => dest.Apellido, opt => opt.MapFrom(src => src.Apellido))
                           .ForMember(dest => dest.NumeroCuenta, opt => opt.MapFrom(src => src.NumeroCuenta));
            #endregion

            #region CuentaAhorro
            CreateMap<SaveCuentasAhorroViewModel, CuentasAhorro>()
                .ForMember(dest => dest.Balance, opt => opt.MapFrom(src => src.Balance))
                .ForMember(dest => dest.EsPrincipal, opt => opt.MapFrom(src => src.EsPrincipal))
                .ForMember(dest => dest.NumeroCuenta, opt => opt.MapFrom(src => src.NumeroCuenta))
                .ForMember(dest => dest.IdProductoFinanciero, opt => opt.MapFrom(src => src.IdProductoFinanciero));

            CreateMap<CuentasAhorro, CuentasAhorroViewModel>()
                .ForMember(dest => dest.NumeroProducto, opt => opt.MapFrom(src => src.ProductoFinanciero.NumeroProducto))
                .ForMember(dest => dest.IdentificadorUnico, opt => opt.MapFrom(src => src.ProductoFinanciero.IdentificadorUnico))
                .ForMember(dest => dest.Balance, opt => opt.MapFrom(src => src.Balance))
                .ForMember(dest => dest.EsPrincipal, opt => opt.MapFrom(src => src.EsPrincipal))
                .ForMember(dest => dest.NumeroCuenta, opt => opt.MapFrom(src => src.NumeroCuenta));
            #endregion
        }
    }
}
