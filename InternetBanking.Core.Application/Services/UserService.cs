using AutoMapper;
using InternetBanking.Core.Application.DTOS.Account.Authentication;
using InternetBanking.Core.Application.DTOS.Account.Edit;
using InternetBanking.Core.Application.DTOS.Account.Register;
using InternetBanking.Core.Application.DTOS.Update;
using InternetBanking.Core.Application.Interfaces.Services.Account;
using InternetBanking.Core.Application.Interfaces.Services.User;
using InternetBanking.Core.Application.ViewModels.User;


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
            var users = await _accountService.GetAllUsersAsync();

            var userList = _mapper.Map<List<UserListViewModel>>(users);

            return userList;
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
           // var dto = _mapper.Map<EditUserDTO>(vm);
            return await _accountService.UpdateUserAndAccountAsync(vm);
        }

        #endregion
    }
}
