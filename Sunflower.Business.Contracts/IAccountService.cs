using System.Threading.Tasks;

namespace Sunflower.Business.Contracts
{
    /// <summary>
    /// Defines operations to manage accounts of the system.
    /// </summary>
    public interface IAccountService
    {
        /// <summary>
        /// Changes the password of the specified account.
        /// </summary>
        /// <param name="email">E-Mail Address of the account of which to change the password.</param>
        /// <param name="newPassword">Password that should be used for the account.</param>
        /// <returns>A Task that will complete when the password has been changed.</returns>
        Task ChangePassword(string email, string newPassword);

        /// <summary>
        /// Checks a password for the specified account.
        /// </summary>
        /// <param name="email">E-Mail Address of the account to check the password for.</param>
        /// <param name="password">Password that should match the password of the account.</param>
        /// <returns>True, if the password matches the password of the account; otherwise false.</returns>
        Task<bool> CheckAccountPassword(string email, string password);

        /// <summary>
        /// Creates a new account.
        /// </summary>
        /// <param name="email">E-Mail Address that the account should be registered with.</param>
        /// <param name="password">Password of the account.</param>
        /// <returns>A Task that will complete when the account has been created.</returns>
        Task CreateAccount(string email, string password);
    }
}