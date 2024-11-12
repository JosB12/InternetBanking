using InternetBanking.Core.Application.DTOS.Account.Authentication;
using InternetBanking.Core.Application.DTOS.Account.Register;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InternetBanking.Core.Application.Interfaces.Services.Account
{
    public interface IAccountService
    {
        Task<int> ObtenerTotalClientesActivosAsync();
        Task<int> ObtenerTotalClientesInactivosAsync();
        Task<AuthenticationResponse> AuthenticateAsync(AuthenticationRequest request);
        Task<RegisterResponse> RegisterBasicUserAsync(RegisterRequest request, string origin);
        Task SignOutAsync();
    }
}
