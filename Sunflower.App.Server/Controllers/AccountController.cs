using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Sunflower.Business.Contracts;
using Sunflower.App.Server.Application.Authentication;

namespace Sunflower.App.Server.Controllers
{
    /// <summary>
    /// API-Controller with endpoints for managing accounts.
    /// </summary>
    public class AccountController : Controller
    {
        private readonly IAccountService _accountService;
        private readonly IAuthenticatorProvider _authenticatorProvider;

        /// <summary>
        /// Creates an AccountController with its dependencies.
        /// </summary>
        /// <param name="accountService">AccountService that should be injected.</param>
        /// <param name="authenticatorProvider">AuthenticationProvider that should be injected.</param>
        public AccountController(IAccountService accountService, IAuthenticatorProvider authenticatorProvider)
        {
            _accountService = accountService;
            _authenticatorProvider = authenticatorProvider;
        }

        /// <summary>
        /// Registers a new user by creating an account.
        /// </summary>
        /// <param name="email">E-Mail address of the new user.</param>
        /// <param name="password">Password of the account.</param>
        [HttpPost]
        public async Task Register(string email, string password)
        {
            await _accountService.CreateAccount(email, password);
        }

        /// <summary>
        /// Logs into the app with the specified account.
        /// </summary>
        /// <param name="email">E-Mail address of the user.</param>
        /// <param name="password">Password of the account.</param>
        [HttpPost]
        public async Task<bool> Login(string email, string password)
        {
            if (!await CheckPassword(email, password)) return false;

            var authenticator = _authenticatorProvider.Provide(HttpContext);
            await authenticator.SignIn(email);
            return true;
        }

        /// <summary>
        /// Changes the password of the account that is currently signed in.
        /// </summary>
        /// <param name="oldPassword">Current password of the account.</param>
        /// <param name="newPassword">New password that should be used.</param>
        [HttpPost]
        public async Task<bool> ChangePassword(string oldPassword, string newPassword)
        {
            var email = _authenticatorProvider.Provide(HttpContext).Email;

            if (!await CheckPassword(email, oldPassword)) return false;

            await _accountService.ChangePassword(email, newPassword);
            return true;
        }

        /// <summary>
        /// Checks if the specified E-Mail address and password combination is valid.
        /// </summary>
        /// <param name="email">E-Mail address of the user.</param>
        /// <param name="password">Password of the account.</param>
        /// <returns>A task that completes with true, if the combination is valid; otherwise false.</returns>
        private async Task<bool> CheckPassword(string email, string password)
        {
            return await _accountService.CheckAccountPassword(email, password);
        }
    }
}