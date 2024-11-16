using InternetBanking.Core.Application.Interfaces.Services;
using InternetBanking.Core.Application.ViewModels.Transacciones;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Identity;
using InternetBanking.Infrastructure.Identity.Entities;

public class TransaccionesController : Controller
{
    private readonly ITransaccionesService _transaccionesService;
    private readonly UserManager<ApplicationUser> _userManager;

    public TransaccionesController(ITransaccionesService transaccionesService, UserManager<ApplicationUser> userManager)
    {
        _transaccionesService = transaccionesService;
        _userManager = userManager;
    }

    [HttpGet]
    public async Task<IActionResult> Transferencia()
    {
        var userId = HttpContext.Session.GetString("userId");
        if (string.IsNullOrEmpty(userId))
        {
            var user = await _userManager.GetUserAsync(User);
            userId = user.Id;
            HttpContext.Session.SetString("userId", userId);
        }

        var cuentas = await _transaccionesService.GetCuentasByUserIdAsync(userId);
        var viewModel = new TransferenciaViewModel
        {
            IdUsuario = userId,
            Cuentas = cuentas
        };
        return View(viewModel);
    }

    [HttpPost]
    public async Task<IActionResult> Transferencia(TransferenciaViewModel transferenciaVm)
    {
        var userId = HttpContext.Session.GetString("userId");
        if (string.IsNullOrEmpty(userId))
        {
            var user = await _userManager.GetUserAsync(User);
            userId = user.Id;
            HttpContext.Session.SetString("userId", userId);
        }

        if (!ModelState.IsValid)
        {
            transferenciaVm.Cuentas = await _transaccionesService.GetCuentasByUserIdAsync(userId);
            return View(transferenciaVm);
        }

        transferenciaVm.IdUsuario = userId; 

        var result = await _transaccionesService.TransferirFondos(transferenciaVm);
        if (!result)
        {
            ModelState.AddModelError("", "Saldo insuficiente o error en las cuentas.");
            transferenciaVm.Cuentas = await _transaccionesService.GetCuentasByUserIdAsync(userId);
            return View(transferenciaVm);
        }

        ViewBag.SuccessMessage = "Transferencia realizada con éxito.";
        transferenciaVm.Cuentas = await _transaccionesService.GetCuentasByUserIdAsync(userId);
        return View(transferenciaVm);
    }

    public IActionResult TransferenciaExitosa()
    {
        ViewBag.SuccessMessage = TempData["SuccessMessage"];
        return View();
    }
}
