using Sunflower.App.Server.Application.Authentication;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace Sunflower.App.Server.Tests.Mocks
{
    /// <summary>
    /// Mocks an AuthenticatorProvider, providing itself as the Authenticator.
    /// </summary>
    public class AuthenticatorProviderMock : IAuthenticatorProvider, IAuthenticator
    {
        /// <summary>
        /// E-Mail address of the current user; or null if not signed in.
        /// </summary>
        public string Email { get; set; }

        /// <summary>
        /// True, if a user is currently signed in; otherwise false.
        /// </summary>
        public bool IsAuthenticated { get; set; }

        /// <summary>
        /// Provides an Authenticator.
        /// </summary>
        /// <param name="context">Context to use for authentication.</param>
        /// <returns>Itself, serving as an Authenticator.</returns>
        public IAuthenticator Provide(HttpContext context)
        {
            return this;
        }

        /// <summary>
        /// Signs in with the specified E-Mail address.
        /// </summary>
        /// <param name="email">E-Mail address of the user that should be signed in.</param>
        /// <returns>A task that completes when the SignIn has completed.</returns>
        public Task SignIn(string email)
        {
            IsAuthenticated = true;
            Email = email;
            return Task.FromResult(0);
        }

        /// <summary>
        /// Clears the authentication information.
        /// </summary>
        /// <returns>A task that completes when the SignOut has completed.</returns>
        public Task SignOut()
        {
            IsAuthenticated = false;
            Email = null;
            return Task.FromResult(0);
        }
    }
}