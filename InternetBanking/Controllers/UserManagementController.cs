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
using InternetBanking.Core.Domain.Enums;
using InternetBanking.Infrastructure.Persistence.Repositories;
using InternetBanking.Core.Application.ViewModels;
using InternetBanking.Core.Application.DTOS.Update;

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
        private readonly ITarjetasCreditoRepository _tarjetasCreditoRepository;
        private readonly IPrestamosRepository _prestamosRepository;
        private readonly IProductosFinancierosRepository _productosFinancierosRepository;

        public UserManagementController(
            IHttpContextAccessor httpContextAccessor,   
            IUserService userService,
            ICuentasAhorroRepository cuentasAhorrorepository,
            UserManager<ApplicationUser> userManager,
            IProductosFinancierosRepository productosFinancierosRepository,
            IPrestamosRepository prestamosRepository,
            ITarjetasCreditoRepository tarjetasCreditoRepository) 
        {
            _userService = userService;
            _cuentasAhorrorepository = cuentasAhorrorepository;
            _userManager = userManager;
            _productosFinancierosRepository = productosFinancierosRepository;
            _httpContextAccessor = httpContextAccessor;
            _tarjetasCreditoRepository = tarjetasCreditoRepository;
            _prestamosRepository = prestamosRepository;
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


        #region mostrarUsuario/Productos

       
        [HttpGet]
        public async Task<IActionResult> ManageProducts(string userId)
        {
            // Obtener el usuario utilizando el servicio
            var userDto = await _userService.GetUserByIdAsync(userId);
            if (userDto == null)
            {
                TempData["Error"] = "Usuario no encontrado.";
                return RedirectToAction(nameof(Index));
            }
            Console.WriteLine($"Usuario encontrado: {userDto}");

            var userViewModel = new UserViewModel
            {
                Id = userDto.Id,
                Nombre = userDto.Nombre,
                Apellido = userDto.Apellido,
                Cedula = userDto.Cedula,
                TipoUsuario = userDto.TipoUsuario,
                EstaActivo = userDto.EstaActivo
            };

            // Obtener los productos asociados al usuario desde el repositorio
            var productos = await _productosFinancierosRepository.GetByUserIdAsync(userId);

            if (productos == null || productos.Count == 0)
            {
                TempData["Info"] = "No se han agregado productos aún.";
            }

            // Mapear los productos a la vista modelo
            var productosViewModel = productos.Select(p => new MostrarProductoViewModel
            {
                TipoProducto = p.TipoProducto.ToString(),
                IdentificadorUnico = p.IdentificadorUnico,
                NumeroProducto = p.NumeroProducto,
                CuentaAhorro = p.TipoProducto == TipoProducto.CuentaAhorro ? new MostrarCuentaAhorroViewModel
                {
                    NumeroCuenta = p.CuentaAhorro?.NumeroCuenta,
                    Balance = p.CuentaAhorro?.Balance ?? 0
                } : null,
                TarjetaCredito = p.TipoProducto == TipoProducto.TarjetaCredito ? new MostrarTarjetaCreditoViewModel
                {
                    NumeroTarjeta = p.TarjetaCredito?.NumeroTarjeta,
                    LimiteCredito = p.TarjetaCredito?.LimiteCredito ?? 0,
                    DeudaActual = p.TarjetaCredito?.DeudaActual ?? 0
                } : null,
                Prestamo = p.TipoProducto == TipoProducto.Prestamo ? new MostrarPrestamoViewModel
                {
                    MontoPrestamo = p.Prestamo?.MontoPrestamo ?? 0,
                    DeudaRestante = p.Prestamo?.DeudaRestante ?? 0
                } : null
            }).ToList();

            // Asignar los productos al ViewModel
            userViewModel.Productos = productosViewModel;

            return View(userViewModel);
        }


        #endregion

        #region add y remove

        [HttpPost]
        public async Task<IActionResult> AddProduct(string userId, TipoProducto tipoProducto, decimal? limiteCredito = null, decimal? montoPrestamo = null)
        {
            if (string.IsNullOrEmpty(userId))
            {
                TempData["Error"] = "ID de usuario no válido";
                return RedirectToAction(nameof(Index));
            }

            var response = await _userService.AddProductToUserAsync(userId, tipoProducto, limiteCredito, montoPrestamo);

            if (response.HasError)
            {
                TempData["Error"] = response.Error;
            }
            else
            {
                TempData["Success"] = "Producto agregado exitosamente al usuario.";
            }

            return RedirectToAction("ManageProducts", new { userId = userId });
        }

        [HttpPost]
        public async Task<IActionResult> RemoveProduct(string userId, string productoId)
        {
            // Validar que los IDs no estén vacíos
            if (string.IsNullOrEmpty(userId) || string.IsNullOrEmpty(productoId))
            {
                TempData["Error"] = "ID de usuario o producto no válido.";
                return RedirectToAction(nameof(Index));
            }

            // Llamar al servicio para eliminar el producto
            var response = await _userService.RemoveProductFromUserAsync(userId, productoId);

            if (response.HasError)
            {
                TempData["Error"] = response.Error;
            }
            else
            {
                TempData["Success"] = "Producto eliminado exitosamente.";
            }

            return RedirectToAction("ManageProducts", new { userId = userId });
        }


      


        #endregion



    }
}
