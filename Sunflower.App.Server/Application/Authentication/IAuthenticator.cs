using System.Threading.Tasks;

namespace Sunflower.App.Server.Application.Authentication
{
    /// <summary>
    /// Handles operations regarding authentication.
    /// </summary>
    public interface IAuthenticator
    {
        /// <summary>
        /// True, if the context has an authenticated user; otherwise false..
        /// </summary>
        bool IsAuthenticated { get; }

        /// <summary>
        /// Gets the email address of the currently authenticated user.
        /// </summary>
        string Email { get; }

        /// <summary>
        /// Signs into the current context with the specified email address.
        /// </summary>
        /// <param name="email">Email Address that should be used for signing ing.</param>
        /// <returns>A task that will complete when the user has been signed in.</returns>
        Task SignIn(string email);

        /// <summary>
        /// Signs out of the current context.
        /// </summary>
        /// <returns>A task that will complete when the user has been signed out.</returns>
        Task SignOut();
    }
}