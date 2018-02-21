using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Http;
using System;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Sunflower.App.Server.Application.Authentication
{
    /// <summary>
    /// Handles authentication using cookies.
    /// </summary>
    public class CookieAuthenticator : IAuthenticator
    {
        /// <summary>
        /// Creates a <see cref="CookieAuthenticator"/> from the specified <see cref="HttpContext"/>.  
        /// </summary>
        /// <param name="context"><see cref="HttpContext"/> that should be used by the <see cref="CookieAuthenticator"/>.</param>
        public CookieAuthenticator(HttpContext context)
        {
            if (context == null) throw new ArgumentNullException(nameof(context));

            Context = context;
            var authenticatedIdentity = GetAuthenticatedIdentity(context);

            if (authenticatedIdentity != null)
            {
                IsAuthenticated = true;
                Email = GetClaimValueFromIdentity(authenticatedIdentity, ClaimTypes.Email);
            }
        }

        /// <summary>
        /// Gets the <see cref="HttpContext"/> used by this <see cref="CookieAuthenticator"/>.
        /// </summary>
        public HttpContext Context { get; }

        /// <summary>
        /// Gets the email address of the currently authenticated user.
        /// </summary>
        public string Email { get; private set; }

        /// <summary>
        /// True, if the context has an authenticated user; otherwise false..
        /// </summary>
        public bool IsAuthenticated { get; private set; }

        /// <summary>
        /// Signs into the current context with the specified email address.
        /// </summary>
        /// <param name="email">Email Address that should be used for signing ing.</param>
        /// <returns>A task that will complete when the user has been signed in.</returns>
        public async Task SignIn(string email)
        {
            var claimsPrincipal = new ClaimsPrincipal(new ClaimsIdentity(new[]
            {
                new Claim(ClaimTypes.Email, email)
            }, CookieAuthenticationDefaults.AuthenticationScheme));

            await Context.SignInAsync(
                CookieAuthenticationDefaults.AuthenticationScheme,
                claimsPrincipal);

            IsAuthenticated = true;
            Email = email;
        }

        /// <summary>
        /// Signs out of the current context.
        /// </summary>
        /// <returns>A task that will complete when the user has been signed out.</returns>
        public async Task SignOut()
        {
            await Context.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);

            IsAuthenticated = false;
            Email = null;
        }

        private static ClaimsIdentity GetAuthenticatedIdentity(HttpContext context)
            => context.User?.Identities?.FirstOrDefault(i => i.IsAuthenticated);

        private static string GetClaimValueFromIdentity(ClaimsIdentity identity, string type)
            => identity.Claims.FirstOrDefault(i => i.Type == type)?.Value;
    }
}