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
        
    }
}
