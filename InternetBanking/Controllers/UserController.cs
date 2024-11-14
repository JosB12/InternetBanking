using Microsoft.AspNetCore.Mvc;
using InternetBanking.Core.Application.Helpers;
using InternetBanking.Core.Application.ViewModels.User;
using WebApp.InternetBanking.Middlewares;
using InternetBanking.Core.Application.Interfaces.Services.User;
using InternetBanking.Core.Application.DTOS.Account.Authentication;



namespace WebApp.InternetBanking.Controllers
{
    public class UserController : Controller
    {
        private readonly IUserService _userService;
        public UserController(IUserService userService)
        {
            _userService = userService;
        }

        [ServiceFilter(typeof(LoginAuthorize))]
        public IActionResult Index()
        {
            return View(new LogginViewModel());
        }

        [ServiceFilter(typeof(LoginAuthorize))]
        [HttpPost]
        public async Task<IActionResult> Index(LogginViewModel vm)
        {
            if (!ModelState.IsValid)
            {
                return View(vm);
            }

              AuthenticationResponse userVm = await _userService.LogginAsync(vm);
    if (userVm != null && userVm.HasError != true)
    {
        HttpContext.Session.Set<AuthenticationResponse>("user", userVm);

        // Verificar si el usuario es administrador
        if (userVm.Roles.Contains("Administrador"))
        {
            return RedirectToRoute(new { controller = "Dashboard", action = "Index" });
        }
        else if (userVm.Roles.Contains("Cliente"))
        {
            HttpContext.Session.SetString("userId", userVm.Id);
            return RedirectToRoute(new { controller = "DashboardCliente", action = "Index" });
        }
        else
        {
            return RedirectToAction("AccessDenied");
        }
    }
    else
    {
        vm.HasError = userVm.HasError;
        vm.Error = userVm.Error;
        return View(vm);
    }
        }
        public async Task<IActionResult> LogOut()
        {
            await _userService.SignOutAsync();
            HttpContext.Session.Remove("user");
            return RedirectToRoute(new { controller = "User", action = "Index" });
        }
        public IActionResult AccessDenied()
        {
            return View();
        }
        
    }
}
