using InternetBanking.Core.Application.Interfaces.Services;
using InternetBanking.Core.Application.ViewModels.Pago;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Identity;
using InternetBanking.Infrastructure.Identity.Entities;
using System.Threading.Tasks;

namespace InternetBanking.Controllers
{
    public class PagoController : Controller
    {
        private readonly IPagoService _pagoService;
        private readonly UserManager<ApplicationUser> _userManager;

        public PagoController(IPagoService pagoService, UserManager<ApplicationUser> userManager)
        {
            _pagoService = pagoService;
            _userManager = userManager;
        }

        [HttpGet]
        public async Task<IActionResult> PagoExpreso()
        {
            var user = await _userManager.GetUserAsync(User);
            var cuentas = await _pagoService.GetCuentasByUserIdAsync(user.Id);
            ViewBag.Cuentas = cuentas;
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> PagoExpreso(SavePagoExpresoViewModel model)
        {
            if (!ModelState.IsValid)
            {
                var user = await _userManager.GetUserAsync(User);
                var cuentas = await _pagoService.GetCuentasByUserIdAsync(user.Id);
                ViewBag.Cuentas = cuentas;
                return View(model);
            }

            var result = await _pagoService.RealizarPagoExpreso(model);
            if (result != "Pago realizado con éxito.")
            {
                ModelState.AddModelError("", result);
                var user = await _userManager.GetUserAsync(User);
                var cuentas = await _pagoService.GetCuentasByUserIdAsync(user.Id);
                ViewBag.Cuentas = cuentas;
                return View(model);
            }

            return RedirectToAction("Index", "DashboardCliente");
        }

        [HttpGet]
        public async Task<IActionResult> PagoTarjetaCredito()
        {
            var user = await _userManager.GetUserAsync(User);
            var cuentas = await _pagoService.GetCuentasByUserIdAsync(user.Id);
            var tarjetas = await _pagoService.GetTarjetasByUserIdAsync(user.Id);
            ViewBag.Cuentas = cuentas;
            ViewBag.Tarjetas = tarjetas;
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> PagoTarjetaCredito(SavePagoViewModel model)
        {
            if (!ModelState.IsValid)
            {
                var user = await _userManager.GetUserAsync(User);
                var cuentas = await _pagoService.GetCuentasByUserIdAsync(user.Id);
                var tarjetas = await _pagoService.GetTarjetasByUserIdAsync(user.Id);
                ViewBag.Cuentas = cuentas;
                ViewBag.Tarjetas = tarjetas;
                return View(model);
            }

            var result = await _pagoService.RealizarPagoTarjetaCredito(model);
            if (result != "Pago realizado con éxito.")
            {
                ModelState.AddModelError("", result);
                var user = await _userManager.GetUserAsync(User);
                var cuentas = await _pagoService.GetCuentasByUserIdAsync(user.Id);
                var tarjetas = await _pagoService.GetTarjetasByUserIdAsync(user.Id);
                ViewBag.Cuentas = cuentas;
                ViewBag.Tarjetas = tarjetas;
                return View(model);
            }

            return RedirectToAction("Index", "DashboardCliente");
        }
    }
}
