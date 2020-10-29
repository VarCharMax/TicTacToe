using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Identity;
using System.Threading.Tasks;

namespace TicTacToe.Services
{
    public interface IExernalUserService
    {
        Task<SignInResult> ExternalLoginSigninAsync(string loginProvider, string providerKey, bool isPersistent);
        Task<AuthenticationProperties> GetExternalAuthenticationProperties(string provider, string redirectUrl);
        Task<ExternalLoginInfo> GetExternaLoginInfoAsync();
    }
}