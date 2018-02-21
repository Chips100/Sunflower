using Microsoft.AspNetCore.Http;

namespace Sunflower.App.Server.Application.Authentication
{
    /// <summary>
    /// Represents a source for <see cref="IAuthenticator"/> .
    /// </summary>
    public interface IAuthenticatorProvider
    {
        /// <summary>
        /// Creates an <see cref="IAuthenticator"/> from the specified <see cref="HttpContext"/>.  
        /// </summary>
        /// <param name="context">Context that should be used by the <see cref="IAuthenticator"/>.</param>
        /// <returns>An <see cref="IAuthenticator"/> for authentication operations.</returns>
        IAuthenticator Provide(HttpContext context);
    }
}