using InternetBanking.Core.Application.ViewModels.User;
using InternetBanking.Core.Application.Services;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace InternetBanking.Controllers
{
    public class AdminController : Controller
    {
        private readonly UserService _userService;

        // Inyección de dependencias
        public AdminController(UserService userService)
        {
            _userService = userService;
        }

        // GET: Admin/Index
        public async Task<IActionResult> Index()
        {
            // Llamamos al método GetAllUsuarios para obtener la lista de usuarios
            var users = await _userService.GetAllUsuarios();

            // Retornamos los usuarios a la vista
            return View(users);
        }
    }
}
