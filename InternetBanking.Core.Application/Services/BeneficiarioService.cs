using AutoMapper;
using InternetBanking.Core.Application.DTOS.Account.Authentication;
using InternetBanking.Core.Application.Helpers;
using InternetBanking.Core.Application.Interfaces.Repositories;
using InternetBanking.Core.Application.Interfaces.Services;
using InternetBanking.Core.Application.Interfaces.Services.Account;
using InternetBanking.Core.Application.Interfaces.Services.User;
using InternetBanking.Core.Application.ViewModels.Beneficiario;
using InternetBanking.Core.Application.ViewModels.User;
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
        public async Task<Beneficiarios> CrearBeneficiarioAsync(SaveBeneficiarioViewModel viewModel)
        {
            try
            {
                if (string.IsNullOrEmpty(viewModel.NumeroCuenta))
                {
                    throw new ArgumentException("El número de cuenta no puede ser nulo o vacío.");
                }

                viewModel.NumeroCuenta = viewModel.NumeroCuenta.PadLeft(9, '0');

                // 3. Validar que la cuenta de ahorro exista
                var cuentaAhorro = await _cuentasAhorroRepository.GetAccountByNumeroCuentaAsync(viewModel.NumeroCuenta);
                if (cuentaAhorro == null)
                {
                    throw new KeyNotFoundException($"La cuenta con número {viewModel.NumeroCuenta} no existe.");
                }

                var beneficiario = _mapper.Map<Beneficiarios>(viewModel);

                beneficiario.CuentaBeneficiario = cuentaAhorro;

                await _beneficiarioRepository.AddAsync(beneficiario);

                return beneficiario;
            }
            catch (ArgumentException ex)
            {
                throw new ArgumentException($"Error en la creación del beneficiario: {ex.Message}", ex);
            }
            catch (KeyNotFoundException ex)
            {
                // Lanzar la excepción para que el controlador la maneje adecuadamente
                throw new KeyNotFoundException($"Error en la creación del beneficiario: {ex.Message}", ex);
            }
            catch (Exception ex)
            {
                // Lanzar una excepción genérica para cualquier otro error
                throw new ApplicationException("Hubo un error inesperado al procesar la creación del beneficiario.", ex);
            }
        }

        public async Task<List<BeneficiarioViewModel>> GetBeneficiariosAsync()
        {
            try
            {
                var beneficiarios = await _beneficiarioRepository.GetAllAsync();

                // Filtrar beneficiarios por el usuario actual
                var beneficiariosUsuario = beneficiarios
                    .Where(b => b.IdUsuario == userViewModel.Id)
                    .Select(b => new BeneficiarioViewModel
                    {
                        Id = b.Id,
                        Nombre = b.Nombre,
                        Apellido = b.Apellido,
                        NumeroCuenta = b.NumeroCuenta
                    })
                    .ToList();

                return beneficiariosUsuario;
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
