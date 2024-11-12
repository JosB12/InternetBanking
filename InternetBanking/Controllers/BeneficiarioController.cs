using InternetBanking.Core.Application.Interfaces.Services;
using InternetBanking.Core.Application.ViewModels.Beneficiario;
using InternetBanking.Core.Application.ViewModels.User;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace InternetBanking.Controllers
{
    [Authorize]
    public class BeneficiarioController : Controller
    {
        private readonly IBeneficiarioService _beneficiarioService;

        public BeneficiarioController(IBeneficiarioService beneficiarioService)
        {
            _beneficiarioService = beneficiarioService;
        }
        public async Task<IActionResult> Index()
        {
            var beneficiarios = await _beneficiarioService.GetBeneficiariosAsync();
            return View(beneficiarios);
        }
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
                // Si el modelo no es válido, vuelve a mostrar el formulario con los mensajes de error
                return View(vm);
            }

            try
            {
                // Obtiene el ID del usuario autenticado
                var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
                if (string.IsNullOrEmpty(userId))
                {
                    throw new UnauthorizedAccessException("El usuario no está autenticado.");
                }

                // Limpia el NumeroCuenta para evitar espacios extra
                var numeroCuentaTrimmed = vm.NumeroCuenta?.Trim();

                // Llama al servicio para agregar el beneficiario, pasando el número de cuenta limpio
                var beneficiario = await _beneficiarioService.CrearBeneficiarioAsync(vm);

                // Redirige a la lista de beneficiarios después de agregar
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                // Muestra el mensaje de error en la vista
                ModelState.AddModelError("", ex.Message);
                return View(vm);
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
