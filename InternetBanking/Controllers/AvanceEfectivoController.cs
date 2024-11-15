using InternetBanking.Core.Application.Interfaces.Services;
using InternetBanking.Core.Application.ViewModels;
using InternetBanking.Core.Application.Helpers;
using InternetBanking.Core.Application.DTOS.Account;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using System;
using System.Linq;
using InternetBanking.Core.Application.Enums;
using InternetBanking.Core.Application.Interfaces.Repositories;
using Microsoft.AspNetCore.Mvc.Rendering;
using InternetBanking.Core.Application.Services;

namespace InternetBanking.Controllers
{
    [Authorize(Roles = "Cliente")]
    public class AvanceEfectivoController : Controller
    {
        private readonly IAvancesEfectivoService _avanceEfectivoService;
        private readonly ITarjetasCreditoRepository _tarjetasCreditoRepository;
        private readonly ICuentasAhorroRepository _cuentasAhorroRepository;
        private readonly IProductosFinancierosRepository _productosFinancierosRepository;

        public AvanceEfectivoController(
            IAvancesEfectivoService avanceEfectivoService,
            ITarjetasCreditoRepository tarjetasCreditoRepository,
            ICuentasAhorroRepository cuentasAhorroRepository,
            IProductosFinancierosRepository productosFinancierosRepository)
        {
            _avanceEfectivoService = avanceEfectivoService;
            _tarjetasCreditoRepository = tarjetasCreditoRepository;
            _cuentasAhorroRepository = cuentasAhorroRepository;
            _productosFinancierosRepository = productosFinancierosRepository;
        }

        #region Crear avance de efectivo
        [HttpGet]
        public async Task<IActionResult> Create()
        {
            try
            {
                var tarjetasCredito = await _tarjetasCreditoRepository.GetAllAsync();
                var cuentasAhorro = await _cuentasAhorroRepository.GetAllAsync();

                // Crear SelectLists para los campos de selección
                ViewData["TarjetaCreditoOptions"] = new SelectList(tarjetasCredito, "Id", "NumeroTarjeta");
                ViewData["CuentaAhorroOptions"] = new SelectList(cuentasAhorro, "Id", "NumeroCuenta");

                return View(new AvanceEfectivoViewModel());
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", "Error al cargar los datos: " + ex.Message);
                return View(new AvanceEfectivoViewModel());
            }
        }


        [HttpPost]
        public async Task<IActionResult> Create(AvanceEfectivoViewModel viewModel)
        {
            if (!ModelState.IsValid)
            {
                return View(viewModel); // Si el modelo no es válido, vuelve a mostrar la vista con los errores
            }

            try
            {
                var tarjetaCredito = await _tarjetasCreditoRepository.GetByIdAsync(viewModel.TarjetaCreditoId);
                var cuentaAhorro = await _cuentasAhorroRepository.GetByIdAsync(viewModel.CuentaAhorroId);

                if (tarjetaCredito == null || cuentaAhorro == null)
                {
                    ModelState.AddModelError("", "Tarjeta de crédito o cuenta de ahorro no válidos.");
                    return View(viewModel); // Si no se encuentran las entidades, devuelve los errores
                }

                if (viewModel.Monto > tarjetaCredito.LimiteCredito)
                {
                    ModelState.AddModelError("", "El monto solicitado excede el límite de crédito de la tarjeta.");
                    return View(viewModel); // Si el monto excede el límite, muestra el error
                }

                var response = await _avanceEfectivoService.RealizarAvanceAsync(viewModel.TarjetaCreditoId, viewModel.CuentaAhorroId, viewModel.Monto);

                if (!response.Success)
                {
                    ModelState.AddModelError("", response.Message); // Si hay un error en el servicio, muestra el mensaje
                    return View(viewModel);
                }

                TempData["Success"] = "Avance de efectivo realizado exitosamente.";
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", "Error al procesar el avance de efectivo: " + ex.Message);
                return View(viewModel); // Si ocurre un error en la ejecución, muestra el mensaje
            }
        }



        #endregion

        #region Mostrar avances de efectivo

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            try
            {
                var avances = await _avanceEfectivoService.GetAllAvancesAsync();
                var viewModel = new List<AvanceEfectivoListadoViewModel>();

                foreach (var avance in avances)
                {
                    var tarjetaCredito = await _tarjetasCreditoRepository.GetByIdAsync(avance.TarjetaCreditoId);
                    var cuentaAhorro = await _cuentasAhorroRepository.GetByIdAsync(avance.CuentaAhorroId);

                    // Verificamos que los objetos no sean nulos
                    if (tarjetaCredito != null && cuentaAhorro != null)
                    {
                        var item = new AvanceEfectivoListadoViewModel
                        {
                            NumeroTarjeta = tarjetaCredito.NumeroTarjeta, // Accedemos a la propiedad
                            NumeroCuenta = cuentaAhorro.NumeroCuenta, // Accedemos a la propiedad
                            Monto = avance.Monto
                        };

                        viewModel.Add(item);
                    }
                }

                return View(viewModel);
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", "Error al obtener los avances: " + ex.Message);
                return View(new List<AvanceEfectivoViewModel>());
            }
        }


        #endregion
    }
}
