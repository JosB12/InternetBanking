using AutoMapper;
using InternetBanking.Core.Application.DTOS.Account.Authentication;
using InternetBanking.Core.Application.Helpers;
using InternetBanking.Core.Application.Interfaces.Repositories;
using InternetBanking.Core.Application.Interfaces.Services;
using InternetBanking.Core.Application.Interfaces.Services.Account;
using InternetBanking.Core.Application.Interfaces.Services.User;
using InternetBanking.Core.Application.ViewModels.Beneficiario;
using InternetBanking.Core.Domain.Entities;
using Microsoft.AspNetCore.Http;

namespace InternetBanking.Core.Application.Services
{
    public class BeneficiarioService : GenericService<SaveBeneficiarioViewModel, 
        BeneficiarioViewModel, 
        Beneficiarios>, 
        IBeneficiarioService
    {

        private readonly IBeneficiarioRepository _beneficiarioRepository;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly AuthenticationResponse userViewModel;
        private readonly IAccountService _accountService;
        private readonly IMapper _mapper;
        private readonly ICuentasAhorroRepository _cuentasAhorroRepository;
        private readonly IUserService _userService;


        public BeneficiarioService(IUserService userService, 
            IHttpContextAccessor httpContextAccessor, 
            IAccountService accountService, 
            IBeneficiarioRepository beneficiarioRepository,
            ICuentasAhorroRepository cuentasAhorroRepository,
            IMapper mapper) 
            : base(beneficiarioRepository, mapper)
        {
            _userService = userService;
            _beneficiarioRepository = beneficiarioRepository;
            _mapper = mapper;
            _httpContextAccessor = httpContextAccessor;
            userViewModel = _httpContextAccessor.HttpContext.Session.Get<AuthenticationResponse>("user");
            _accountService = accountService;
        }
        public async Task<BeneficiarioViewModel> AddBeneficiarioAsync(SaveBeneficiarioViewModel vm)
        {
            try
            {
                var cuenta = await _cuentasAhorroRepository.GetAccountByNumeroCuentaAsync(vm.NumeroCuenta);
                if (cuenta == null)
                {
                    throw new Exception("La cuenta de beneficiario no existe.");
                }

                if (cuenta.ProductoFinanciero.IdUsuario == userViewModel.Id)
                {
                    throw new Exception("No puedes agregar tu propia cuenta como beneficiario.");
                }

                var beneficiarioExistente = await _beneficiarioRepository.GetAllAsync();
                if (beneficiarioExistente.Any(b => b.IdUsuario == userViewModel.Id && b.NumeroCuenta == vm.NumeroCuenta))
                {
                    throw new Exception("Este beneficiario ya está registrado.");
                }

                var usuarioBeneficiario = await _userService.GetUserDetailsAsync(cuenta.ProductoFinanciero.IdUsuario);

                var beneficiario = new Beneficiarios
                {
                    IdUsuario = userViewModel.Id,
                    IdCuentaBeneficiario = cuenta.Id,
                    NumeroCuenta = cuenta.NumeroCuenta,
                    Nombre = usuarioBeneficiario.Nombre,
                    Apellido = usuarioBeneficiario.Apellido
                };

                var savedBeneficiario = await _beneficiarioRepository.AddAsync(beneficiario);
                return _mapper.Map<BeneficiarioViewModel>(savedBeneficiario);
            }
            catch (Exception ex)
            {
                throw new Exception($"Error al agregar beneficiario: {ex.Message}");
            }
        }

        public async Task<List<BeneficiarioViewModel>> GetBeneficiariosAsync()
        {
            try
            {
                var beneficiarios = await _beneficiarioRepository.GetAllAsync();
                var beneficiariosUsuario = beneficiarios.Where(b => b.IdUsuario == userViewModel.Id).ToList();

                var beneficiarioViewModels = new List<BeneficiarioViewModel>();

                foreach (var beneficiario in beneficiariosUsuario)
                {
                    var viewModel = new BeneficiarioViewModel
                    {
                        Id = beneficiario.Id,
                        Nombre = beneficiario.Nombre,
                        Apellido = beneficiario.Apellido,
                        NumeroCuenta = beneficiario.NumeroCuenta
                    };

                    beneficiarioViewModels.Add(viewModel);
                }

                return beneficiarioViewModels;
            }
            catch (Exception ex)
            {
                throw new Exception($"Error al obtener beneficiarios: {ex.Message}");
            }
        }

        public async Task DeleteBeneficiarioAsync(int id)
        {
            try
            {
                var beneficiario = await _beneficiarioRepository.GetByIdAsync(id);
                if (beneficiario == null)
                {
                    throw new Exception("Beneficiario no encontrado.");
                }

                if (beneficiario.IdUsuario != userViewModel.Id)
                {
                    throw new Exception("No tienes permiso para eliminar este beneficiario.");
                }

                await _beneficiarioRepository.DeleteAsync(beneficiario);
            }
            catch (Exception ex)
            {
                throw new Exception($"Error al eliminar beneficiario: {ex.Message}");
            }
        }




    }
}
