using InternetBanking.Core.Application.DTOS.Account.Authentication;
using InternetBanking.Core.Application.Enums;
using InternetBanking.Core.Application.Interfaces.Services.User;
using InternetBanking.Core.Application.ViewModels.User;
using InternetBanking.Infrastructure.Identity.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using InternetBanking.Core.Application.Helpers;
using System.Security.Claims;
using InternetBanking.Core.Application.Interfaces.Repositories;
using Azure;

namespace InternetBanking.Controllers
{
    [Authorize(Roles = "Administrador,SuperAdmin")]
    public class UserManagementController : Controller
    {
        private readonly IUserService _userService;
        private readonly UserManager<ApplicationUser> _userManager; 
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly AuthenticationResponse userViewModel;
        private readonly ICuentasAhorroRepository _cuentasAhorrorepository;

        public UserManagementController(
            IHttpContextAccessor httpContextAccessor,   
            IUserService userService,
            ICuentasAhorroRepository cuentasAhorrorepository,
            UserManager<ApplicationUser> userManager) 
        {
            _userService = userService;
            _cuentasAhorrorepository = cuentasAhorrorepository;
            _userManager = userManager;
            _httpContextAccessor = httpContextAccessor;
            userViewModel = _httpContextAccessor.HttpContext.Session.Get<AuthenticationResponse>("user");



        }
        #region index cretae
        public async Task<IActionResult> Index()
        {
            try
            {
                var users = await _userService.GetAllUsersForViewAsync();
                return View(users);
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", "Error al obtener los usuarios: " + ex.Message);
                return View(new List<UserListViewModel>());
            }
        }
        [HttpGet]
        public IActionResult Create()
        {
            return View(new SaveUserViewModel());
        }

        [HttpPost]
        public async Task<IActionResult> Create(SaveUserViewModel vm)
        {
            if (!ModelState.IsValid)
            {
                return View(vm);
            }

                if (vm.TipoUsuario == TipoUsuario.Cliente && !vm.MontoInicial.HasValue)
                {
                    ModelState.AddModelError("MontoInicial", "El monto inicial es obligatorio para los clientes.");
                    return View(vm);
                }
            try
            {

                var registerResponse = await _userService.RegisterAsync(vm);

                if (registerResponse.HasError)
                {
                    ModelState.AddModelError("", registerResponse.Error);
                    return View(vm);
                }

                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                string errorDetails = ex.InnerException != null ? ex.InnerException.Message : ex.Message;
                ModelState.AddModelError("", "Error al crear el usuario: " + errorDetails);
                return View(vm);
            }
        }
        #endregion

        #region Activate/Deactivate User

        [HttpPost]
        public async Task<IActionResult> Deactivate(string userId)
        {
            if (string.IsNullOrEmpty(userId))
            {
                TempData["Error"] = "ID de usuario no válido";
                return RedirectToAction(nameof(Index));
            }

            var loggedInUserId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            var response = await _userService.DeactivateUserAsync(userId, loggedInUserId);

            if (response.HasError)
            {
                TempData["Error"] = response.Error;
            }
            else
            {
                TempData["Success"] = "Usuario desactivado exitosamente";
            }

            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        public async Task<IActionResult> Activate(string userId)
        {
            if (string.IsNullOrEmpty(userId))
            {
                TempData["Error"] = "ID de usuario no válido";
                return RedirectToAction(nameof(Index));
            }

            var loggedInUserId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            var response = await _userService.ActivateUserAsync(userId, loggedInUserId);

            if (response.HasError)
            {
                TempData["Error"] = response.Error;
            }
            else
            {
                TempData["Success"] = "Usuario activado exitosamente";
            }

            return RedirectToAction(nameof(Index));
        }


        #endregion

        #region Editar usuario

        [HttpGet]
        public async Task<IActionResult> Edit(string Id)
        {
            var user = await _userService.GetUserForEditViewAsync(Id);
            if (user == null)
            {
                return RedirectToAction("Index");
            }
          

            return View(user);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(EditProfileViewModel vm)
        {
            // Validar las contraseñas solo si se está intentando cambiar la contraseña
            if (!string.IsNullOrEmpty(vm.Password) || !string.IsNullOrEmpty(vm.ConfirmPassword))
            {
                if (vm.Password != vm.ConfirmPassword)
                {
                    ModelState.AddModelError("ConfirmPassword", "Las contraseñas no coinciden.");
                    return View(vm);
                }
            }

            if (!ModelState.IsValid)
            {
                return View(vm);
            }

            var success = await _userService.UpdateUserAndAccountAsync(vm);
            if (!success)
            {
                TempData["Error"] = "Error al actualizar el usuario.";
                return View(vm);
            }

            TempData["Success"] = "Usuario actualizado exitosamente.";
            return RedirectToAction("Index");
        }
        #endregion

    }
}
