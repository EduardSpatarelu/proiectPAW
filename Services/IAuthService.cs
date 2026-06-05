using BARmonica.Web.Models;
using Microsoft.AspNetCore.Identity;

namespace BARmonica.Web.Services
{
    public interface IAuthService
    {
        Task<IdentityResult> RegisterAsync(RegisterViewModel model);
        Task<SignInResult> LoginAsync(LoginViewModel model);
        Task LogoutAsync();
    }
}