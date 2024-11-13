using InternetBanking.Core.Application.DTOS.Account.Authentication;
using InternetBanking.Core.Application.DTOS.Account.Edit;
using InternetBanking.Core.Application.DTOS.Account.Get;
using InternetBanking.Core.Application.DTOS.Account.Register;
using InternetBanking.Core.Application.DTOS.Update;
using InternetBanking.Core.Application.ViewModels.User;
using InternetBanking.Core.Domain.Enums;


namespace InternetBanking.Core.Application.Interfaces.Services.Account
{
    public interface IAccountService
    {
        Task<int> ObtenerTotalClientesActivosAsync();
        Task<int> ObtenerTotalClientesInactivosAsync();
        Task<AuthenticationResponse> AuthenticateAsync(AuthenticationRequest request);
        Task<RegisterResponse> RegisterUserAsync(RegisterRequest request);
        Task<List<UserDTO>> GetAllUsersAsync();
        Task<UserDTO> GetUserByIdAsync(string userId);
        Task<bool> IsLoggedUserAdminAsync(string userId);
        Task<UpdateUserResponse> DeactivateUserAsync(string userId, string loggedInUserId);
        Task<UpdateUserResponse> ActivateUserAsync(string userId, string loggedInUserId);
        Task<EditUserDTO> GetUserForEditAsync(string userId);
        Task<bool> UpdateUserAndAccountAsync(EditProfileViewModel vm);
        Task SignOutAsync();
        Task<UpdateUserResponse> AddProductToUserAsync(string userId, TipoProducto tipoProducto, decimal? limiteCredito = null, decimal? montoPrestamo = null);
        Task<UpdateUserResponse> RemoveProductFromUserAsync(string userId, string productoId);

    }
}
