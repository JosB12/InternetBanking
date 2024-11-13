using InternetBanking.Core.Application.DTOS.Account.Authentication;
using InternetBanking.Core.Application.DTOS.Account.Edit;
using InternetBanking.Core.Application.DTOS.Account.Get;
using InternetBanking.Core.Application.DTOS.Account.Register;
using InternetBanking.Core.Application.DTOS.Update;
using InternetBanking.Core.Application.Enums;
using InternetBanking.Core.Application.Interfaces.Repositories;
using InternetBanking.Core.Application.Interfaces.Services.Account;
using InternetBanking.Core.Application.ViewModels.User;
using InternetBanking.Core.Domain.Entities;
using InternetBanking.Core.Domain.Enums;
using InternetBanking.Infrastructure.Identity.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace InternetBanking.Infrastructure.Identity.Services
{
    public class AccountService : IAccountService
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly IProductosFinancierosRepository _productosFinancierosRepository;
        private readonly ICuentasAhorroRepository _cuentasAhorroRepository;
        private readonly ITarjetasCreditoRepository _tarjetasCreditoRepository;
        private readonly IPrestamosRepository _prestamosRepository;
        public AccountService(UserManager<ApplicationUser> userManager, 
            SignInManager<ApplicationUser> signInManager,
            IProductosFinancierosRepository productosFinancierosRepository,
            ICuentasAhorroRepository cuentasAhorroRepository,
            ITarjetasCreditoRepository tarjetasCreditoRepository,
            IPrestamosRepository prestamosRepository)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _productosFinancierosRepository = productosFinancierosRepository;
            _cuentasAhorroRepository = cuentasAhorroRepository;
            _tarjetasCreditoRepository = tarjetasCreditoRepository;
            _prestamosRepository = prestamosRepository;
        }
        #region Extra
        public async Task<AuthenticationResponse> GetUserByUsernameAsync(string username)
        {
            var user = await _userManager.Users
                .FirstOrDefaultAsync(u => u.UserName == username);

            if (user == null)
            {
                return null;
            }

            return new AuthenticationResponse
            {
                Id = user.Id,
                UserName = user.UserName,
                Email = user.Email,
                Roles = (await _userManager.GetRolesAsync(user)).ToList(),
                HasError = false
            };
        }
        #endregion


        #region login\register\logout
        public async Task<AuthenticationResponse> AuthenticateAsync(AuthenticationRequest request)
        {

            AuthenticationResponse response = new();

            var user = await _userManager.FindByNameAsync(request.UserName);
            if (user == null)
            {
                response.HasError = true;
                response.Error = $"No Accounts registered with {request.UserName}";
                return response;
            }

            var result = await _signInManager.PasswordSignInAsync(user.UserName, request.Password, false, lockoutOnFailure: false);
            if (!result.Succeeded)
            {
                response.HasError = true;
                response.Error = $"Invalid credentials for {request.UserName}";
                return response;
            }
            if (!user.EstaActivo)
            {
                response.HasError = true;
                response.Error = $"Account no confirmed for {request.UserName}";
                return response;
            }
            response.Id = user.Id;
            response.Email = user.Email;
            response.UserName = user.UserName;

            var rolesList = await _userManager.GetRolesAsync(user).ConfigureAwait(false);

            response.Roles = rolesList.ToList();

            return response;
        }

        public async Task SignOutAsync()
        {
            await _signInManager.SignOutAsync();
        }

        public async Task<RegisterResponse> RegisterUserAsync(RegisterRequest request)
        {
            var response = new RegisterResponse { HasError = false };

            var existingUser = await _userManager.FindByNameAsync(request.UserName);
            if (existingUser != null)
            {
                response.HasError = true;
                response.Error = "Username already taken.";
                return response;
            }

            var user = new ApplicationUser
            {
                Email = request.Email,
                Nombre = request.Nombre,
                Apellido = request.Apellido,
                UserName = request.UserName,
                Cedula = request.Cedula,
                TipoUsuario = request.TipoUsuario 
            };

            var result = await _userManager.CreateAsync(user, request.Password);
            if (!result.Succeeded)
            {
                response.HasError = true;
                response.Error = "Error creating user.";
                return response;
            }

            // Asignar el rol según el tipo de usuario
            string role = request.TipoUsuario switch
            {
                TipoUsuario.Administrador => "Administrador",
                TipoUsuario.Cliente => "Cliente",
                TipoUsuario.superadmin => "SuperAdmin",
                _ => throw new ArgumentException("Tipo de usuario no válido")
            };

            await _userManager.AddToRoleAsync(user, role);

            if (request.TipoUsuario == TipoUsuario.Cliente)
            {
                var productoFinanciero = new ProductosFinancieros
                {
                    IdUsuario = user.Id,
                    TipoProducto = TipoProducto.CuentaAhorro,
                    IdentificadorUnico = GenerarIdentificadorUnico(),
                    NumeroProducto = GenerarNumeroProducto(),
                    FechaCreacion = DateTime.Now
                };

                await _productosFinancierosRepository.AddAsync(productoFinanciero);

                var cuentaAhorro = new CuentasAhorro
                {
                    IdentificadorUnico = GenerarIdentificadorUnico(),
                    NumeroCuenta = GenerarNumeroCuenta(),
                    Balance = request.MontoInicial ?? 0,
                    EsPrincipal = true,
                    IdProductoFinanciero = productoFinanciero.Id
                };

                await _cuentasAhorroRepository.AddAsync(cuentaAhorro);
            }

            return response;
        }

        #endregion

        #region metodos de apoyo
        private string GenerarIdentificadorUnico()
        {
            return new Random().Next(100000000, 999999999).ToString();
        }
        public string GenerarNumeroTarjeta()
        {
            var random = new Random();

            long numeroTarjeta = 0;

            for (int i = 0; i < 16; i++)
            {
                numeroTarjeta = numeroTarjeta * 10 + random.Next(0, 10); 
            }

            return numeroTarjeta.ToString();
        }

        private string GenerarNumeroCuenta()
        {
            return Guid.NewGuid().ToString("N").Substring(0, 20);
        }
        private string GenerarNumeroProducto()
        {
            return Guid.NewGuid().ToString("N").Substring(0, 20); 
        }
        #endregion

        #region get
        public async Task<List<UserDTO>> GetAllUsersAsync()
        {
            var users = await _userManager.Users
                .OrderByDescending(u => u.FechaCreacion)
                .ToListAsync();

            var userDTOList = new List<UserDTO>();

            foreach (var user in users)
            {
                var roles = await _userManager.GetRolesAsync(user);
                userDTOList.Add(new UserDTO
                {
                    Id = user.Id,
                    Nombre = user.Nombre,
                    Apellido = user.Apellido,
                    Cedula = user.Cedula,
                    Email = user.Email,
                    TipoUsuario = roles.Contains("Administrador") ? "Administrador" : "Cliente",
                    EstaActivo = user.EstaActivo
                });
            }

            return userDTOList;
        }
        public async Task<UserDTO> GetUserByIdAsync(string userId)
        {
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
            {
                Console.WriteLine($"Usuario con ID {userId} no encontrado.");
                return null;
            }
            Console.WriteLine($"Usuario encontrado: {user.Id}, {user.Nombre}, {user.TipoUsuario}, {user.EstaActivo}");

            var roles = await _userManager.GetRolesAsync(user);
            return new UserDTO
            {
                Id = user.Id,
                Nombre = user.Nombre,
                Apellido = user.Apellido,
                Cedula = user.Cedula,
                Email = user.Email,
                TipoUsuario = roles.Contains("Administrador") ? "Administrador" : "Cliente",
                EstaActivo = user.EstaActivo
            };
        }
        #endregion

        #region desactivar activar UsuarioLogeado
        public async Task<bool> IsLoggedUserAdminAsync(string userId)
        {
            var loggedUser = await _userManager.GetUserAsync(_signInManager.Context.User);
            return loggedUser != null &&
                   loggedUser.Id == userId &&
                   await _userManager.IsInRoleAsync(loggedUser, "Administrador");
        }

        public async Task<UpdateUserResponse> DeactivateUserAsync(string userId, string loggedInUserId)
        {
            // Cambio en la lógica: verificar si el usuario intenta desactivar su propia cuenta
            if (userId == loggedInUserId)
            {
                var loggedUser = await _userManager.FindByIdAsync(loggedInUserId);
                if (loggedUser != null && await _userManager.IsInRoleAsync(loggedUser, "Administrador"))
                {
                    return new UpdateUserResponse
                    {
                        HasError = true,
                        Error = "El administrador logeado no puede deactivar su propia cuenta"
                    };
                }
            }

            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
            {
                return new UpdateUserResponse
                {
                    HasError = true,
                    Error = "Usuario no encontrado"
                };
            }

            user.EstaActivo = false;
            var result = await _userManager.UpdateAsync(user);
            if (result.Succeeded)
            {
                return new UpdateUserResponse { HasError = false };
            }

            return new UpdateUserResponse
            {
                HasError = true,
                Error = "Error al tratar de desactivar el usuario"
            };
        }

        public async Task<UpdateUserResponse> ActivateUserAsync(string userId, string loggedInUserId)
        {
            // Misma lógica que DeactivateUserAsync
            if (userId == loggedInUserId)
            {
                var loggedUser = await _userManager.FindByIdAsync(loggedInUserId);
                if (loggedUser != null && await _userManager.IsInRoleAsync(loggedUser, "Administrador"))
                {
                    return new UpdateUserResponse
                    {
                        HasError = true,
                        Error = "El administrador logeado no puede activar su propia cuenta"
                    };
                }
            }

            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
            {
                return new UpdateUserResponse
                {
                    HasError = true,
                    Error = "Usuario no encontrado"
                };
            }

            user.EstaActivo = true;
            var result = await _userManager.UpdateAsync(user);
            if (result.Succeeded)
            {
                return new UpdateUserResponse { HasError = false };
            }

            return new UpdateUserResponse
            {
                HasError = true,
                Error = "Error al tratar de activar el usuario"
            };
        }

        #endregion


        #region Editar Usuario
        public async Task<EditUserDTO> GetUserForEditAsync(string userId)
        {
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null) return null;

            return new EditUserDTO
            {
                Id = user.Id,
                Nombre = user.Nombre,
                Apellido = user.Apellido,
                Cedula = user.Cedula,
                Email = user.Email,
                UserName = user.UserName,
                TipoUsuario = user.TipoUsuario, // Añadimos el TipoUsuario para diferenciar entre cliente y administrador
                MontoAdicional = null // Campo para clientes, solo visible en la vista si el usuario es cliente
             
            };
        }
        public async Task<bool> UpdateUserAndAccountAsync(EditProfileViewModel vm)
        {
            // Obtiene el usuario
            var user = await _userManager.FindByIdAsync(vm.Id);
            if (user == null) return false;

            // Actualiza la información del usuario
            user.Nombre = !string.IsNullOrWhiteSpace(vm.Nombre) ? vm.Nombre : user.Nombre;
            user.Apellido = !string.IsNullOrWhiteSpace(vm.Apellido) ? vm.Apellido : user.Apellido;
            user.Cedula = !string.IsNullOrWhiteSpace(vm.Cedula) ? vm.Cedula : user.Cedula;
            user.Email = !string.IsNullOrWhiteSpace(vm.Email) ? vm.Email : user.Email;
            user.UserName = !string.IsNullOrWhiteSpace(vm.UserName) ? vm.UserName : user.UserName;

            

            var savingsAccount = await _cuentasAhorroRepository.GetSavingsAccountByUserIdAsync(user.Id);
            if (vm.MontoAdicional.HasValue && vm.MontoAdicional.Value > 0)
            {
                // Si el usuario es Cliente, se crea un Producto Financiero de tipo Cuenta de Ahorro
                if (user.TipoUsuario == TipoUsuario.Cliente)
                {
                    // Crear un nuevo Producto Financiero de tipo Cuenta de Ahorro
                    var nuevoProducto = new ProductosFinancieros
                    {
                        IdUsuario = user.Id,
                        TipoProducto = TipoProducto.CuentaAhorro,
                        FechaCreacion = DateTime.Now,
                        IdentificadorUnico = GenerarIdentificadorUnico(),
                        NumeroProducto = GenerarNumeroProducto()
                    };

                    // Guardar el Producto Financiero
                    await _productosFinancierosRepository.AddAsync(nuevoProducto);

                    // Crear la cuenta de ahorro asociada al producto financiero
                    var nuevaCuentaAhorro = new CuentasAhorro
                    {
                        Balance = vm.MontoAdicional.Value,
                        EsPrincipal = true,
                        NumeroCuenta = GenerarNumeroCuenta(),
                        IdentificadorUnico = GenerarIdentificadorUnico(),
                        IdProductoFinanciero = nuevoProducto.Id  // Asociar la cuenta al Producto Financiero
                    };

                    // Guardar la cuenta de ahorro
                    await _cuentasAhorroRepository.AddAsync(nuevaCuentaAhorro);
                }
            }

            // Actualiza el MontoInicial solo si el usuario es Cliente
            if (vm.TipoUsuario == TipoUsuario.Cliente)
            {
                user.MontoInicial = vm.MontoAdicional;
            }

            if (!string.IsNullOrWhiteSpace(vm.Password))
            {
                var token = await _userManager.GeneratePasswordResetTokenAsync(user);
                await _userManager.ResetPasswordAsync(user, token, vm.Password);
            }
            var result = await _userManager.UpdateAsync(user);
            return result.Succeeded;
        }


        #endregion

        #region Agregar y quitar producto a usuario
        
        public async Task<UpdateUserResponse> AddProductToUserAsync(string userId, TipoProducto tipoProducto, decimal? limiteCredito = null, decimal? montoPrestamo = null)
        {
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
            {
                return new UpdateUserResponse
                {
                    HasError = true,
                    Error = "Usuario no encontrado."
                };
            }

            var identificadorUnico = GenerarIdentificadorUnico();
            var producto = new ProductosFinancieros
            {
                IdUsuario = user.Id,
                TipoProducto = tipoProducto,
                IdentificadorUnico = identificadorUnico,
                NumeroProducto = GenerarNumeroProducto(),
                FechaCreacion = DateTime.Now
            };

            // Guardar producto financiero en la base de datos
            await _productosFinancierosRepository.AddAsync(producto);

            if (tipoProducto == TipoProducto.CuentaAhorro)
            {
                var cuentaAhorro = new CuentasAhorro
                {
                    IdProductoFinanciero = producto.Id,
                    NumeroCuenta = GenerarNumeroCuenta(),
                    Balance = 0,
                    IdentificadorUnico = GenerarIdentificadorUnico(),
                    EsPrincipal = false 
                };
                await _cuentasAhorroRepository.AddAsync(cuentaAhorro);


            }
            if (tipoProducto == TipoProducto.TarjetaCredito)
            {
                if (!limiteCredito.HasValue || limiteCredito <= 0)
                {
                    return new UpdateUserResponse
                    {
                        HasError = true,
                        Error = "El límite de crédito debe ser mayor a 0 para una tarjeta de crédito."
                    };
                }

                var tarjetaCredito = new TarjetasCredito
                {
                    IdProductoFinanciero = producto.Id,
                    NumeroTarjeta = GenerarNumeroTarjeta(),
                    LimiteCredito = limiteCredito.Value,
                    DeudaActual = 0, // Inicializar deuda en 0
                    IdentificadorUnico = GenerarIdentificadorUnico()
                };
                await _tarjetasCreditoRepository.AddAsync(tarjetaCredito);
            }

            // Validación y creación de Préstamo
            if (tipoProducto == TipoProducto.Prestamo)
            {
                if (!montoPrestamo.HasValue || montoPrestamo <= 0)
                {
                    return new UpdateUserResponse
                    {
                        HasError = true,
                        Error = "El monto del préstamo debe ser mayor a 0."
                    };
                }

                var prestamo = new Prestamos
                {
                    IdProductoFinanciero = producto.Id,
                    MontoPrestamo = montoPrestamo.Value,
                    DeudaRestante = montoPrestamo.Value,
                    FechaInicio = DateTime.Now
                };
                await _prestamosRepository.AddAsync(prestamo);

                // Actualizar el balance de la cuenta de ahorro principal
                var cuentaPrincipal = await _cuentasAhorroRepository.GetPrincipalAccountByUserIdAsync(userId);
                if (cuentaPrincipal != null)
                {
                    cuentaPrincipal.Balance += montoPrestamo.Value;
                    await _cuentasAhorroRepository.CuentasAhorroUpdateAsync(cuentaPrincipal);
                }
            }

            return new UpdateUserResponse
            {
                HasError = false
            };
        }

        public async Task<UpdateUserResponse> RemoveProductFromUserAsync(string userId, string productoId)
        {
            var producto = await _productosFinancierosRepository.GetByIdentificadorUnicoAsync(productoId);

            if (producto == null)
            {
                return new UpdateUserResponse
                {
                    HasError = true,
                    Error = "Producto no encontrado o no pertenece al usuario."
                };
            }

            if (producto.IdUsuario != userId)
            {
                return new UpdateUserResponse
                {
                    HasError = true,
                    Error = "El producto no pertenece al usuario."
                };
            }

            try
            {
                // Lógica para eliminar el producto dependiendo de su tipo
                if (producto.TipoProducto == TipoProducto.CuentaAhorro)
                {
                    var cuentaAhorro = await _cuentasAhorroRepository.GetByProductoIdAsync(producto.Id);
                   
                    if (cuentaAhorro != null)
                    {
                        Console.WriteLine($"CuentaAhorro Id: {cuentaAhorro.Id}, EsPrincipal: {cuentaAhorro.EsPrincipal}");

                        if (cuentaAhorro.EsPrincipal == true)
                        {
                            return new UpdateUserResponse
                            {
                                HasError = true,
                                Error = "No se puede eliminar una cuenta de ahorro principal."
                            };
                        }
                        Console.WriteLine($"CuentaAhorro Id: {cuentaAhorro.Id}, EsPrincipal: {cuentaAhorro.EsPrincipal}");

                        await _cuentasAhorroRepository.DeleteAsync(cuentaAhorro);
                    }
                }
                else if (producto.TipoProducto == TipoProducto.TarjetaCredito)
                {
                    var tarjetaCredito = await _tarjetasCreditoRepository.GetByProductoFinancieroIdAsync(producto.Id);
                    if (tarjetaCredito != null)
                    {
                        // Verificación de deuda pendiente
                        if (tarjetaCredito.DeudaActual > 0)
                        {
                            return new UpdateUserResponse
                            {
                                HasError = true,
                                Error = "No se puede eliminar la tarjeta de crédito porque tiene deuda pendiente."
                            };
                        }

                        await _tarjetasCreditoRepository.DeleteAsync(tarjetaCredito);
                    }
                }
                else if (producto.TipoProducto == TipoProducto.Prestamo)
                {
                    var prestamo = await _prestamosRepository.GetByProductoFinancieroIdAsync(producto.Id);
                    if (prestamo != null)
                    {
                        // Verificación de deuda pendiente
                        if (prestamo.DeudaRestante > 0)
                        {
                            return new UpdateUserResponse
                            {
                                HasError = true,
                                Error = "No se puede eliminar el préstamo porque tiene deuda pendiente."
                            };
                        }

                        await _prestamosRepository.DeleteAsync(prestamo);
                    }
                }

                // Eliminar el producto financiero
                await _productosFinancierosRepository.DeleteAsync(producto);
                return new UpdateUserResponse
                {
                    HasError = false
                };
            }
            catch (Exception ex)
            {
                return new UpdateUserResponse
                {
                    HasError = true,
                    Error = $"Error al eliminar el producto: {ex.Message}"
                };
            }
        }

        public async Task<int> ObtenerTotalClientesActivosAsync()
        {
            // Obtiene todos los usuarios activos
            var usuariosActivos = await _userManager.Users
                .Where(u => u.EstaActivo)
                .ToListAsync();

            int count = 0;
            // Filtra los usuarios activos que tienen el rol de "Cliente"
            foreach (var user in usuariosActivos)
            {
                if (await _userManager.IsInRoleAsync(user, "Cliente"))
                {
                    count++;
                }
            }

            return count;
        }

        public async Task<int> ObtenerTotalClientesInactivosAsync()
        {
            // Obtiene todos los usuarios inactivos
            var usuariosInactivos = await _userManager.Users
                .Where(u => !u.EstaActivo)
                .ToListAsync();

            int count = 0;
            // Filtra los usuarios inactivos que tienen el rol de "Cliente"
            foreach (var user in usuariosInactivos)
            {
                if (await _userManager.IsInRoleAsync(user, "Cliente"))
                {
                    count++;
                }
            }

            return count;
        }



        #endregion


    }
}
