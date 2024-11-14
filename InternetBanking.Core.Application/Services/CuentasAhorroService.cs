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
  
    }
}
