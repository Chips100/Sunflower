using Microsoft.AspNetCore.Http;

namespace Sunflower.App.Server.Application.Authentication
{
    /// <summary>
    /// Provides a <see cref="CookieAuthenticator"/> for authentication.
    /// </summary>
    public class CookieAuthenticatorProvider : IAuthenticatorProvider
    {
        /// <summary>
        /// Creates a <see cref="CookieAuthenticator"/> from the specified <see cref="HttpContext"/>.  
        /// </summary>
        /// <param name="context">Context that should be used by the <see cref="CookieAuthenticator"/>.</param>
        /// <returns>A <see cref="CookieAuthenticator"/> for authentication operations.</returns>
        public IAuthenticator Provide(HttpContext context)
        {
            return new CookieAuthenticator(context);
        }
    }
}