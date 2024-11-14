using InternetBanking.Core.Application.DTOS.Account.Authentication;
using InternetBanking.Core.Application.DTOS.Account.Details;
using InternetBanking.Core.Application.DTOS.Account.Get;
using InternetBanking.Core.Application.DTOS.Account.Register;
using InternetBanking.Core.Application.Enums;
using InternetBanking.Core.Application.Interfaces.Services.Account;
using InternetBanking.Infrastructure.Identity.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.EntityFrameworkCore;
using System.Text;

namespace InternetBanking.Infrastructure.Identity.Services
{
    public class AccountService : IAccountService
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        public AccountService(UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
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
                EstaActivo = user.EmailConfirmed.ToString(),
                HasError = false
            };
        }

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
            response.EstaActivo = user.EmailConfirmed.ToString();

            return response;
        }

        public async Task SignOutAsync()
        {
            await _signInManager.SignOutAsync();
        }

        public async Task<RegisterResponse> RegisterBasicUserAsync(RegisterRequest request, string origin)
        {
            RegisterResponse response = new()
            {
                HasError = false
            };

            var userWithSameUserName = await _userManager.FindByNameAsync(request.UserName);
            if (userWithSameUserName != null)
            {
                response.HasError = true;
                response.Error = $"Username '{request.UserName}' is already taken.";
                return response;
            }

            var userWithSameEmail = await _userManager.FindByEmailAsync(request.Email);
            if (userWithSameEmail != null)
            {
                response.HasError = true;
                response.Error = $"Email '{request.Email}' is already registered.";
                return response;
            }

            var user = new ApplicationUser
            {
                Email = request.Email,
                Nombre = request.Nombre,
                Apellido = request.Apellido,
                UserName = request.UserName,
                PhoneNumber = request.Telefono,
                Cedula = request.Cedula,
                EmailConfirmed = true,
                PhoneNumberConfirmed = true
            };

            var result = await _userManager.CreateAsync(user, request.Password);
            if (!result.Succeeded)
            {
                response.HasError = true;
                response.Error = "Error creating user.";
                return response;
            }

            if (request.TipoUsuario == "Admin")
            {
                await _userManager.AddToRoleAsync(user, Roles.Administrador.ToString());
                if (user.UserName == "superadminuser")
                {
                    await _userManager.AddToRoleAsync(user, Roles.SuperAdmin.ToString());
                }
            }
            else if (request.TipoUsuario == "Client")
            {
                await _userManager.AddToRoleAsync(user, Roles.Cliente.ToString());

                // Si el tipo es "Client", asignar un saldo inicial (si existe)
                if (request.MontoInicial.HasValue && request.MontoInicial.Value > 0)
                {
                    // Aquí debes agregar la lógica para guardar el saldo inicial, en base al modelo de tu aplicación
                    // Por ejemplo, crear un objeto `Account` que esté vinculado al usuario.

                    // Crear una cuenta bancaria para el cliente
                    //var account = new Account
                    //{
                    //    Balance = request.InitialAmount.Value, // Asigna el saldo inicial
                    //    UserId = user.Id // Relacionamos la cuenta con el usuario
                    //};

                    // Aquí guardamos la cuenta en la base de datos
                    // Necesitas un repositorio o contexto de base de datos para hacer esto.
                    //await _accountRepository.CreateAsync(account); // Si tienes un repositorio de cuentas
                }
            }

            return response;
        }

        public async Task<string> ConfirmAccountAsync(string userId, string token)
        {
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
            {
                return $"No accounts registered with this user";
            }

            token = Encoding.UTF8.GetString(WebEncoders.Base64UrlDecode(token));
            var result = await _userManager.ConfirmEmailAsync(user, token);
            if (result.Succeeded)
            {
                return $"Account confirmed for {user.Email}. You can now use the app";
            }
            else
            {
                return $"An error occurred while confirming {user.Email}.";
            }
        }
        public async Task<UserDetailsDTO> GetUserDetailsAsync(string userId)
        {
            var user = await _userManager.FindByIdAsync(userId);

            if (user == null)
            {
                throw new Exception("Usuario no encontrado");
            }

            return new UserDetailsDTO
            {
                Nombre = user.Nombre,
                Apellido = user.Apellido
            };
        }
    }
}
