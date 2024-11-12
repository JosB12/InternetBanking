using InternetBanking.Core.Application.Interfaces.Repositories.CuentaAhorro;
using InternetBanking.Core.Application.Interfaces.Repositories.Transaccion;
using InternetBanking.Core.Application.Interfaces.Services.Transaccion;
using InternetBanking.Core.Application.Interfaces.Services.User;
using InternetBanking.Core.Application.ViewModels.Transferencia;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore; // Necesario para usar Include

namespace InternetBanking.Controllers
{
    public class TransaccionesController : Controller
    {
        private readonly ITransaccionesService _transaccionesService;
        private readonly IUserService _userService;
        private readonly ICuentaAhorroRepository _cuentaAhorroRepository;

        public TransaccionesController(ITransaccionesService transaccionesService, IUserService userService, ICuentaAhorroRepository cuentaAhorroRepository)
        {
            _transaccionesService = transaccionesService;
            _userService = userService;
            _cuentaAhorroRepository = cuentaAhorroRepository;
        }

        public async Task<IActionResult> CrearTransferencia(string UsuarioId)
        {
            // Obtener las cuentas del usuario actual, incluyendo el producto financiero asociado
            var cuentasUsuario = await _cuentaAhorroRepository.ObtenerCuentasPorUsuarioAsync(UsuarioId);

            // Preparar las cuentas para el ViewBag, concatenando el número de cuenta con el nombre del producto
            var cuentasConProducto = cuentasUsuario.Select(c => new
            {
                c.Id,
                CuentaNombre = $"{c.NumeroCuenta} - {c.ProductoFinanciero?.TipoProducto.ToString()}"
            }).ToList();

            // Asignar las cuentas al ViewBag con la lista que contiene la cuenta y el tipo de producto financiero
            ViewBag.CuentasUsuario = new SelectList(cuentasConProducto, "Id", "CuentaNombre");

            return View();
        }

        [HttpPost]
        public async Task<IActionResult> CrearTransferencia(TransaccionesViewModel model)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    // Llamamos al servicio para realizar la transferencia
                    bool transferenciaExitosa = await _transaccionesService.RealizarTransferenciaAsync(model);

                    if (transferenciaExitosa)
                    {
                        TempData["Message"] = "La transferencia se ha realizado con éxito.";
                        return RedirectToAction("Index", "Home"); // Redirigimos al usuario a la página de inicio o a donde deseemos
                    }
                    else
                    {
                        TempData["ErrorMessage"] = "Hubo un error al realizar la transferencia.";
                        return View(model); // Si falla, regresamos a la misma vista con el modelo
                    }
                }
                catch (Exception ex)
                {
                    // Manejo de errores generales
                    TempData["ErrorMessage"] = $"Ocurrió un error: {ex.Message}";
                    return View(model);
                }
            }

            // Si el modelo no es válido, regresamos la vista con los errores
            return View(model);
        }
    }
}
