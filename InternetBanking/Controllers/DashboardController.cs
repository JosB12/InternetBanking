using InternetBanking.Core.Application.Interfaces.Services;
using InternetBanking.Core.Application.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace InternetBanking.Controllers
{
    public class DashboardController : Controller
    {
        private readonly IDashboardService _dashboardService;

        public DashboardController(IDashboardService dashboardService)
        {
            _dashboardService = dashboardService;
        }

        public async Task<IActionResult> Index()
        {
            var viewModel = new DashboardViewModel
            {
                TransaccionesTotales = await _dashboardService.ObtenerCantidadTotalTransaccionesAsync(),
                TransaccionesHoy = await _dashboardService.ObtenerCantidadTransaccionesDelDiaAsync(),
                TotalPagos = await _dashboardService.ObtenerCantidadTotalPagosAsync(),
                PagosHoy = await _dashboardService.ObtenerCantidadPagosDelDiaAsync(),
                ClientesActivos = await _dashboardService.ObtenerCantidadClientesActivosAsync(),
                ClientesInactivos = await _dashboardService.ObtenerCantidadClientesInactivosAsync(),
                ProductosAsignados = await _dashboardService.ObtenerCantidadTotalProductosFinancierosAsync()
            };

            return View(viewModel);
        }
    }

}
