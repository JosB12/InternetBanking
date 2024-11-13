using InternetBanking.Core.Application.Interfaces.Repositories;
using InternetBanking.Core.Application.Interfaces.Services;
using InternetBanking.Core.Application.ViewModels.Beneficiario;
using InternetBanking.Core.Application.ViewModels.User;
using InternetBanking.Infrastructure.Persistence.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace InternetBanking.Controllers
{
    [Authorize]
    public class BeneficiarioController : Controller
    {
        private readonly IBeneficiarioService _beneficiarioService;
        private readonly ICuentasAhorroRepository _cuentasAhorroRepository;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public BeneficiarioController(IBeneficiarioService beneficiarioService, ICuentasAhorroRepository cuentasAhorroRepository , IHttpContextAccessor httpContextAccessor)
        {
            _beneficiarioService = beneficiarioService;
            _cuentasAhorroRepository = cuentasAhorroRepository;
            _httpContextAccessor = httpContextAccessor;
        }
        public async Task<IActionResult> Index()
        {
            var beneficiarios = await _beneficiarioService.GetBeneficiariosAsync();
            return View(beneficiarios);
        }
        [HttpGet]
        public async Task<IActionResult> Create()
        {
            var vm = new SaveBeneficiarioViewModel();
            return View(vm);
        }


        [HttpPost]
        public async Task<IActionResult> Create(SaveBeneficiarioViewModel vm)
        {
            if (!ModelState.IsValid)
            {
                TempData["ErrorMessage"] = "Hay errores en el formulario. Por favor revisa los datos.";
                return RedirectToAction("Index");
            }

            try
            {
                // Obtener la información de la cuenta de ahorro
                var accountData = await _cuentasAhorroRepository.ValidateAccountAsync(vm.NumeroCuenta);

                // Verificar si la cuenta existe
                if (accountData.Equals(default((string NumeroCuenta, int IdProductoFinanciero, string IdUsuario))))
                {
                    TempData["ErrorMessage"] = "La cuenta de ahorro no existe.";
                    return RedirectToAction("Index");
                }

                // Obtener el ID del usuario actual desde la sesión
                var idUsuarioActual = _httpContextAccessor.HttpContext.Session.GetString("UserId");

                // Crear el beneficiario llamando al servicio correspondiente
                var (success, message) = await _beneficiarioService.CrearBeneficiarioAsync(vm.NumeroCuenta, idUsuarioActual);

                // Si hubo un problema, mostrar el mensaje de error en la vista
                if (!success)
                {
                    TempData["ErrorMessage"] = message;
                    return RedirectToAction("Index");
                }

                // Beneficiario agregado exitosamente
                TempData["SuccessMessage"] = "Beneficiario agregado exitosamente.";
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = $"Error al crear el beneficiario: {ex.Message}";
                return RedirectToAction("Index");
            }
        }





        [HttpPost]
        public async Task<IActionResult> Delete(int id)
        {
            await _beneficiarioService.DeleteBeneficiarioAsync(id);

            return RedirectToAction("Index");
        }


    }
}
