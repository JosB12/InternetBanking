using InternetBanking.Core.Application.Interfaces.Services;
using InternetBanking.Core.Application.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace InternetBanking.Controllers
{
    [Authorize(Roles = "Administrador,SuperAdmin,Cliente")]
    public class DashboardClienteController : Controller
    {
        private readonly IDashboardService _dashboardService;
        private readonly IProductosFinancierosService _productosFinancierosService;

        public DashboardClienteController(IDashboardService dashboardService, IProductosFinancierosService productosFinancierosService)
        {
            _dashboardService = dashboardService;
            _productosFinancierosService = productosFinancierosService;
        }

        public async Task<IActionResult> Index()
        {
            var userId = HttpContext.Session.GetString("userId");
            if (string.IsNullOrEmpty(userId))
            {
                return RedirectToAction("Index", "User");
            }

            var productos = await _productosFinancierosService.GetProductosByUserIdAsync(userId);

            var viewModel = new DashboardViewModel
            {
                TransaccionesTotales = await _dashboardService.ObtenerCantidadTotalTransaccionesAsync(),
                TransaccionesHoy = await _dashboardService.ObtenerCantidadTransaccionesDelDiaAsync(),
                TotalPagos = await _dashboardService.ObtenerCantidadTotalPagosAsync(),
                PagosHoy = await _dashboardService.ObtenerCantidadPagosDelDiaAsync(),
                ClientesActivos = await _dashboardService.ObtenerCantidadClientesActivosAsync(),
                ClientesInactivos = await _dashboardService.ObtenerCantidadClientesInactivosAsync(),
                ProductosAsignados = productos.Count,
                Productos = productos
            };

            return View(viewModel);
        }
    }
}