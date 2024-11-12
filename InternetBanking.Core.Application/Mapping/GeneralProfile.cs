using AutoMapper;
using InternetBanking.Core.Application.DTOS.Account.Authentication;
using InternetBanking.Core.Application.DTOS.Account.Register;
using InternetBanking.Core.Application.ViewModels.Transferencia;
using InternetBanking.Core.Application.ViewModels.User;
using InternetBanking.Core.Domain.Entities;
using InternetBanking.Core.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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


            #region Transaccion
            CreateMap<TransaccionesViewModel, Transacciones>()
            .ForMember(dest => dest.TipoTransaccion, opt => opt.MapFrom(src => TipoTransaccion.Transferencia)) // Asignamos el tipo de transacción como "Transferencia"
            .ForMember(dest => dest.IdCuentaOrigen, opt => opt.MapFrom(src => src.CuentaOrigenId))
            .ForMember(dest => dest.IdCuentaDestino, opt => opt.MapFrom(src => src.CuentaDestinoId))
            .ForMember(dest => dest.Monto, opt => opt.MapFrom(src => src.Monto))
            .ForMember(dest => dest.Fecha, opt => opt.MapFrom(src => DateTime.Now)) // Fecha de la transacción
            .ForMember(dest => dest.IdProductoFinanciero, opt => opt.Ignore()) // Ignoramos propiedades que no se usan
            .ForMember(dest => dest.CuentaOrigen, opt => opt.Ignore())
            .ForMember(dest => dest.CuentaDestino, opt => opt.Ignore())
            .ForMember(dest => dest.IdUsuario, opt => opt.Ignore());
            #endregion
        }
    }
}
