using AutoMapper;
using InternetBanking.Core.Application.DTOS.Account.Authentication;
using InternetBanking.Core.Application.Interfaces.Repositories;
using InternetBanking.Core.Application.Interfaces.Services.Account;
using InternetBanking.Core.Application.Interfaces.Services;
using InternetBanking.Core.Domain.Entities;
using Microsoft.AspNetCore.Http;
using InternetBanking.Core.Application.ViewModels.CuentasAhorro;
using InternetBanking.Core.Application.Helpers;

namespace InternetBanking.Core.Application.Services
{
    public class CuentasAhorroService : GenericService<SaveCuentasAhorroViewModel, 
        CuentasAhorroViewModel, 
        CuentasAhorro>, 
        ICuentasAhorroService
    {

        private readonly ICuentasAhorroRepository _cuentasAhorroRepository;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly AuthenticationResponse userViewModel;
        private readonly IAccountService _accountService;
        private readonly IMapper _mapper;

        public CuentasAhorroService(IHttpContextAccessor httpContextAccessor,
            IAccountService accountService,
            ICuentasAhorroRepository cuentasAhorroRepository,
            IMapper mapper)
            : base(cuentasAhorroRepository, mapper)
        {
            _cuentasAhorroRepository = cuentasAhorroRepository;
            _mapper = mapper;
            _httpContextAccessor = httpContextAccessor;
            userViewModel = _httpContextAccessor.HttpContext.Session.Get<AuthenticationResponse>("user");
            _accountService = accountService;
        }
        public async Task<CuentasAhorroViewModel> GetAccountByNumeroCuentaAsync(string numeroCuenta)
        {
            // Validar entrada
            if (string.IsNullOrEmpty(numeroCuenta))
            {
                throw new ArgumentException("El número de cuenta no puede ser vacío.", nameof(numeroCuenta));
            }

            // Obtener la cuenta usando el número de cuenta
            var cuenta = await _cuentasAhorroRepository.GetAccountByNumeroCuentaAsync(numeroCuenta);

            // Verificar si la cuenta existe
            if (cuenta == null)
            {
                throw new KeyNotFoundException($"No se encontró la cuenta con el número de cuenta: {numeroCuenta}");
            }

            // Mapear a ViewModel
            var cuentaViewModel = _mapper.Map<CuentasAhorroViewModel>(cuenta);
            return cuentaViewModel;
        }




    }
}
