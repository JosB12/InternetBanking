using InternetBanking.Core.Application.DTOS.Account.Authentication;
using InternetBanking.Core.Application.DTOS.Account.Register;
using InternetBanking.Core.Application.DTOS.Update;
using InternetBanking.Core.Application.ViewModels.User;


namespace InternetBanking.Core.Application.Interfaces.Services.User
{
    public interface IUserService
    {
        Task<AuthenticationResponse> LogginAsync(LogginViewModel vm);
        Task<RegisterResponse> RegisterAsync(SaveUserViewModel vm);
        Task<List<UserListViewModel>> GetAllUsersForViewAsync();
        Task<UpdateUserResponse> DeactivateUserAsync(string userId, string loggedInUserId);
        Task<UpdateUserResponse> ActivateUserAsync(string userId, string loggedInUserId);
        Task<bool> UpdateUserAndAccountAsync(EditProfileViewModel vm);
        Task<EditProfileViewModel> GetUserForEditViewAsync(string userId);
        Task SignOutAsync();
    }
}
