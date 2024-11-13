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
        public async Task<(bool success, string message)> CrearBeneficiarioAsync(string numeroCuenta, string idUsuarioActual)
        {
            try
            {
                // 1. Validar la cuenta del beneficiario
                var (exists, cuenta, idUsuarioCuenta) = await _cuentasAhorroRepository.ValidateAccountAsync(numeroCuenta);

                if (!exists)
                {
                    return (false, $"La cuenta {numeroCuenta} no existe en el sistema.");
                }

                // 2. Verificar que no se esté agregando su propia cuenta como beneficiario
                if (idUsuarioCuenta == idUsuarioActual)
                {
                    return (false, "No puedes agregar tu propia cuenta como beneficiario.");
                }

                // 3. Verificar si ya existe este beneficiario
                var beneficiarioExistente = await _beneficiarioRepository.AnyAsync(b =>
                    b.IdUsuario == idUsuarioActual &&
                    b.NumeroCuenta == numeroCuenta);

                if (beneficiarioExistente)
                {
                    return (false, "Este beneficiario ya está registrado en tu lista.");
                }

                // 4. Crear el nuevo beneficiario
                var beneficiario = new Beneficiarios
                {
                    IdUsuario = idUsuarioActual,
                    IdCuentaBeneficiario = cuenta.Id,
                    CuentaBeneficiario = cuenta,
                    Nombre = cuenta.ProductoFinanciero?.NumeroProducto ?? "No disponible",
                    Apellido = cuenta.NumeroCuenta ?? "No disponible",
                    NumeroCuenta = numeroCuenta
                };

                // 5. Guardar el beneficiario
                await _beneficiarioRepository.AddAsync(beneficiario);

                return (true, "Beneficiario agregado exitosamente.");
            }
            catch (Exception ex)
            {
                return (false, "Ocurrió un error al procesar la solicitud. Por favor, inténtelo de nuevo.");
            }
        }

        private void ValidarBeneficiario(Beneficiarios beneficiario)
        {
            if (beneficiario == null)
            {
                throw new ArgumentNullException(nameof(beneficiario));
            }

            if (beneficiario.CuentaBeneficiario == null)
            {
                throw new InvalidOperationException("La cuenta del beneficiario no puede ser nula.");
            }

            if (string.IsNullOrWhiteSpace(beneficiario.Nombre))
            {
                throw new InvalidOperationException("El nombre del beneficiario no puede estar vacío.");
            }

            if (string.IsNullOrWhiteSpace(beneficiario.Apellido))
            {
                throw new InvalidOperationException("El apellido del beneficiario no puede estar vacío.");
            }

            // Agregar aquí otras validaciones específicas según tu modelo de Beneficiarios
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
