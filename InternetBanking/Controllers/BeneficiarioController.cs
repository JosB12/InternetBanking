using InternetBanking.Core.Application.DTOS.Account.Authentication;
using InternetBanking.Core.Application.Interfaces.Repositories;
using InternetBanking.Core.Application.Interfaces.Services;
using Microsoft.AspNetCore.Authorization;
using InternetBanking.Core.Application.Helpers;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using InternetBanking.Core.Application.ViewModels.Beneficiario;

namespace InternetBanking.Controllers
{
    [Authorize]
    public class BeneficiarioController : Controller
    {
        private readonly IBeneficiarioService _beneficiarioService;
        private readonly ICuentasAhorroRepository _cuentasAhorroRepository;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly AuthenticationResponse userViewModel;

        public BeneficiarioController(IBeneficiarioService beneficiarioService, ICuentasAhorroRepository cuentasAhorroRepository, IHttpContextAccessor httpContextAccessor)
        {
            _beneficiarioService = beneficiarioService;
            _cuentasAhorroRepository = cuentasAhorroRepository;
            _httpContextAccessor = httpContextAccessor;
            userViewModel = _httpContextAccessor.HttpContext.Session.Get<AuthenticationResponse>("user");

        }
        public async Task<IActionResult> Index()
        {
            var beneficiarios = await _beneficiarioService.GetBeneficiariosAsync();
            return View(beneficiarios);
        }
        [HttpGet]
        public IActionResult Create()
        {
            return View(new SaveBeneficiarioViewModel());
        }

        [HttpPost]
        public async Task<IActionResult> Create(SaveBeneficiarioViewModel vm)
        {
            if (!ModelState.IsValid)
            {
                return View(vm);
            }

            var userId = userViewModel.Id;
            if (string.IsNullOrEmpty(userId))
            {
                return RedirectToAction("Index", "Beneficiario");
            }

            var (success, message) = await _beneficiarioService.AgregarBeneficiarioAsync(vm.NumeroCuenta, userId);

            if (!success)
            {
                ModelState.AddModelError(string.Empty, message);
                return View(vm);
            }

            TempData["SuccessMessage"] = "Beneficiario agregado exitosamente";
            return RedirectToAction(nameof(Index));
        }






        [HttpPost]
        public async Task<IActionResult> Delete(int id)
        {
            await _beneficiarioService.DeleteBeneficiarioAsync(id);

            return RedirectToAction("Index");
        }


    }
}
