using InternetBanking.Core.Application.DTOS.Account.Authentication;
using InternetBanking.Core.Application.DTOS.Account.Edit;
using InternetBanking.Core.Application.DTOS.Account.Get;
using InternetBanking.Core.Application.DTOS.Account.Register;
using InternetBanking.Core.Application.DTOS.Update;
using InternetBanking.Core.Application.ViewModels.User;


namespace InternetBanking.Core.Application.Interfaces.Services.Account
{
    public interface IAccountService
    {
        Task<AuthenticationResponse> AuthenticateAsync(AuthenticationRequest request);
        Task<RegisterResponse> RegisterUserAsync(RegisterRequest request);
        Task<List<UserDTO>> GetAllUsersAsync();
        Task<bool> IsLoggedUserAdminAsync(string userId);
        Task<UpdateUserResponse> DeactivateUserAsync(string userId, string loggedInUserId);
        Task<UpdateUserResponse> ActivateUserAsync(string userId, string loggedInUserId);
        Task<EditUserDTO> GetUserForEditAsync(string userId);
        Task<bool> UpdateUserAndAccountAsync(EditProfileViewModel vm);
        Task SignOutAsync();
    }
}
