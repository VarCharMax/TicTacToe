using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using System.Collections.Generic;
using System.Threading.Tasks;
using TicTacToe.Models;

namespace TicTacToe.Services
{
    public interface IUserService
    {
        Task<bool> IsOnline(string name);
        Task<bool> RegisterUser(UserModel userModel, bool isOnline = false);
        Task<UserModel> GetUserByEmail(string email);
        Task UpdateUser(UserModel user);
        Task<IEnumerable<UserModel>> GetTopUsers(int numberOfUsers);
        Task<string> GetEmailConfirmationCode(UserModel user);
        Task<bool> ConfirmEmail(string email, string code);
        Task<SignInResult> SignInUser(LoginModel loginModel, HttpContext httpContext);
        public Task SignOutUser(HttpContext httpContext);
        public Task<AuthenticationProperties> GetExternalAuthenticationProperties(string provider, string redirectUrl);
        public Task<ExternalLoginInfo> GetExternaLoginInfoAsync();
        public Task<SignInResult> ExternalLoginSigninAsync(string loginProvider, string providerKey, bool isPersistent);
        public Task<IdentityResult> EnableTwoFactor(string name, bool enabled);
        public Task<string> GetTwoFactorCode(string userName, string tokenProvider);
        public Task<bool> ValidateTwoFactor(string userName, string tokenProvider, string token, HttpContext httpContext);
        public Task<string> GetResetPasswordCode(UserModel user);
        public Task<IdentityResult> ResetPassword(string userName, string password, string token);
    }
}