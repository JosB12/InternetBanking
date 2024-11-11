using InternetBanking.Core.Application.DTOS.Account.Authentication;
using InternetBanking.Core.Application.DTOS.Account.Details;
using InternetBanking.Core.Application.DTOS.Account.Register;
using InternetBanking.Core.Application.ViewModels.User;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InternetBanking.Core.Application.Interfaces.Services.User
{
    public interface IUserService
    {
        Task<AuthenticationResponse> LogginAsync(LogginViewModel vm);
        Task<RegisterResponse> RegisterAsync(SaveUserViewModel vm, string origin);
        Task<UserDetailsDTO> GetUserDetailsAsync(string userId);
        Task SignOutAsync();
    }
}
