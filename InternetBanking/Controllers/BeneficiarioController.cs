using InternetBanking.Core.Application.Interfaces.Services;
using InternetBanking.Core.Application.ViewModels.Beneficiario;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

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
                return View(vm);
            }

            var beneficiario = await _beneficiarioService.AddBeneficiarioAsync(vm);
            return RedirectToAction("Index");
        }

        [HttpPost]
        public async Task<IActionResult> Delete(int id)
        {
            await _beneficiarioService.DeleteBeneficiarioAsync(id);

            return RedirectToAction("Index");
        }


    }
}
