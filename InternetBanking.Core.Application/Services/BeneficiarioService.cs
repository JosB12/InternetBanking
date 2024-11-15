using AutoMapper;
using InternetBanking.Core.Application.DTOS.Account.Authentication;
using InternetBanking.Core.Application.Interfaces.Repositories;
using InternetBanking.Core.Application.Interfaces.Services.Account;
using InternetBanking.Core.Application.Interfaces.Services.User;
using InternetBanking.Core.Application.Interfaces.Services;
using InternetBanking.Core.Domain.Entities;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using InternetBanking.Core.Application.Helpers;
using InternetBanking.Core.Application.ViewModels.Beneficiario;

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
            _cuentasAhorroRepository = cuentasAhorroRepository;
        }


        public async Task<(bool success, string message)> AgregarBeneficiarioAsync(string numeroCuenta, string idUsuarioActual)
        {
            try
            {
                // Validar que la cuenta existe
                var (exists, cuenta, idUsuarioCuenta) = await _cuentasAhorroRepository.ValidateAccountAsync(numeroCuenta);

                if (!exists || cuenta == null)
                {
                    return (false, $"La cuenta {numeroCuenta} no existe en el sistema.");
                }

                if (idUsuarioCuenta == idUsuarioActual)
                {
                    return (false, "No puedes agregar tu propia cuenta como beneficiario.");
                }

                // Verificar si el beneficiario ya está registrado
                var beneficiarioExistente = await _beneficiarioRepository.AnyAsync(b =>
                    b.IdUsuario == idUsuarioActual &&
                    b.NumeroCuenta == numeroCuenta);

                if (beneficiarioExistente)
                {
                    return (false, "Este beneficiario ya está registrado en tu lista.");
                }

                // Obtener datos del usuario asociado a la cuenta
                var usuario = await _accountService.GetUserByIdAsync(idUsuarioCuenta);

                if (usuario == null)
                {
                    return (false, "No se pudo obtener la información del usuario asociado a la cuenta.");
                }

                // Crear el beneficiario con datos correctos
                var beneficiario = new Beneficiarios
                {
                    IdUsuario = idUsuarioActual,
                    IdCuentaBeneficiario = cuenta.Id,
                    CuentaBeneficiario = cuenta,
                    Nombre = usuario.Nombre, // Usar el nombre del usuario
                    Apellido = usuario.Apellido, // Usar el apellido del usuario
                    NumeroCuenta = numeroCuenta,
                };

                await _beneficiarioRepository.AddAsync(beneficiario);

                return (true, "Beneficiario agregado exitosamente.");
            }
            catch (Exception ex)
            {
                return (false, $"Ocurrió un error al procesar la solicitud. Detalles: {ex.Message}");
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
