using AutoMapper;
using InternetBanking.Core.Application.DTOS.Account.Authentication;
using InternetBanking.Core.Application.DTOS.Account.Edit;
using InternetBanking.Core.Application.DTOS.Account.Get;
using InternetBanking.Core.Application.DTOS.Account.Register;
using InternetBanking.Core.Application.DTOS.Update;
using InternetBanking.Core.Application.Interfaces.Services;
using InternetBanking.Core.Application.Interfaces.Services.Account;
using InternetBanking.Core.Application.Interfaces.Services.User;
using InternetBanking.Core.Application.ViewModels.User;
using InternetBanking.Core.Domain.Enums;


namespace InternetBanking.Core.Application.Services
{
    public class UserService : IUserService
    {
        private readonly IAccountService _accountService;
        private readonly IMapper _mapper;


        public UserService(IAccountService accountService, IMapper mapper)
        {
            _accountService = accountService;
            _mapper = mapper;
        }

        #region entity Authentication - Authorization
        public async Task<AuthenticationResponse> LogginAsync(LogginViewModel vm)
        {
            AuthenticationRequest logginRequest = _mapper.Map<AuthenticationRequest>(vm);
            AuthenticationResponse userResponse = await _accountService.AuthenticateAsync(logginRequest);
            return userResponse;
        }
        public async Task SignOutAsync()
        {
            await _accountService.SignOutAsync();
        }

        public async Task<RegisterResponse> RegisterAsync(SaveUserViewModel vm)
        {
            var registerRequest = _mapper.Map<RegisterRequest>(vm);

            return await _accountService.RegisterUserAsync(registerRequest);
        }

        #endregion

        #region User Functions
        public async Task<List<UserListViewModel>> GetAllUsersForViewAsync()
        {
            try
            {
                var users = await _accountService.GetAllUsersAsync();
                var userList = _mapper.Map<List<UserListViewModel>>(users);
                return userList;
            }
            catch (Exception ex)
            {
                // Manejar el error, como registrar el error y/o retornar un mensaje
                throw new ApplicationException("Error al obtener usuarios", ex);
            }
        }
       

        #endregion

        #region manejo de Usuario (Activado/Deactivado)
        public async Task<UpdateUserResponse> DeactivateUserAsync(string userId, string loggedInUserId)
        {
            return await _accountService.DeactivateUserAsync(userId, loggedInUserId);
        }

        public async Task<UpdateUserResponse> ActivateUserAsync(string userId, string loggedInUserId)
        {
            return await _accountService.ActivateUserAsync(userId, loggedInUserId);
        }
        #endregion

        #region manejo de usuario Editar 
        public async Task<EditProfileViewModel> GetUserForEditViewAsync(string userId)
        {
            var dto = await _accountService.GetUserForEditAsync(userId);
            return _mapper.Map<EditProfileViewModel>(dto);
        }


        public async Task<bool> UpdateUserAndAccountAsync(EditProfileViewModel vm)
        {
            return await _accountService.UpdateUserAndAccountAsync(vm);
        }

        #endregion

        #region manejo de productos
        public async Task<UserDTO> GetUserByIdAsync(string userId)
        {
            var userDto = await _accountService.GetUserByIdAsync(userId);

            if (userDto == null)
            {
                return null;
            }
            
            Console.WriteLine($"Usuario encontrado: {userDto}");

            return userDto;
        }

        public async Task<UpdateUserResponse> AddProductToUserAsync(string userId, TipoProducto tipoProducto, decimal? limiteCredito = null, decimal? montoPrestamo = null)
        {
            return await _accountService.AddProductToUserAsync(userId, tipoProducto, limiteCredito, montoPrestamo);
        }

        public async Task<UpdateUserResponse> RemoveProductFromUserAsync(string userId, string productoId)
        {
            return await _accountService.RemoveProductFromUserAsync(userId, productoId);
        }
        #endregion
    }   
}
