using Sunflower.Business.Contracts;
using Sunflower.Business.Exceptions;
using Sunflower.Business.Security;
using Sunflower.Data.Contracts;
using Sunflower.Entities;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace Sunflower.Business
{
    /// <summary>
    /// Defines operations to manage accounts of the system.
    /// </summary>
    public class AccountService : IAccountService
    {
        private readonly IEntityRepository<Account> _accountRepository;

        /// <summary>
        /// Creates an AccountService.
        /// </summary>
        /// <param name="accountRepository">Repository to access stored account entities.</param>
        public AccountService(IEntityRepository<Account> accountRepository)
        {
            _accountRepository = accountRepository;
        }

        /// <summary>
        /// Changes the password of the specified account.
        /// </summary>
        /// <param name="email">E-Mail Address of the account of which to change the password.</param>
        /// <param name="newPassword">Password that should be used for the account.</param>
        /// <returns>A Task that will complete when the password has been changed.</returns>
        public async Task ChangePassword(string email, string newPassword)
        {
            var account = await GetAccountByEmail(email);
            var hashedPassword = HashedPassword.CreateFromPlainPassword(newPassword);

            await _accountRepository.Change(account.Id, a =>
            {
                a.PasswordHash = hashedPassword.Hash;
                a.PasswordSalt = hashedPassword.Salt;
            });
        }

        /// <summary>
        /// Checks a password for the specified account.
        /// </summary>
        /// <param name="email">E-Mail Address of the account to check the password for.</param>
        /// <param name="password">Password that should match the password of the account.</param>
        /// <returns>True, if the password matches the password of the account; otherwise false.</returns>
        public async Task<bool> CheckAccountPassword(string email, string password)
        {
            // A non-existing account for the email will be treated as a normal
            // failed password check to not disclose the information about the existence.
            var account = await GetAccountByEmail(email, false);
            if (account == null)
            {
                return false;
            }

            var hashedPassword = new HashedPassword(account.PasswordHash, account.PasswordSalt);
            return hashedPassword.EqualsPlainPassword(password);
        }

        /// <summary>
        /// Creates a new account.
        /// </summary>
        /// <param name="email">E-Mail Address that the account should be registered with.</param>
        /// <param name="password">Password of the account.</param>
        /// <returns>A Task that will complete when the account has been created.</returns>
        public async Task CreateAccount(string email, string password)
        {
            // Check if email address is already used.
            var account = await GetAccountByEmail(email, false);
            if (account != null)
            {
                throw new EmailAlreadyRegisteredException(email);
            }

            // If not, create new account.
            var hashedPassword = HashedPassword.CreateFromPlainPassword(password);

            await _accountRepository.Create(new Account()
            {
                EmailAddress = email,
                PasswordHash = hashedPassword.Hash,
                PasswordSalt = hashedPassword.Salt
            });
        }

        /// <summary>
        /// Gets an account by its email address.
        /// </summary>
        /// <param name="email">email address of the account to get.</param>
        /// <param name="throwIfNotFound">If true, no exception will be thrown if the account could not be found.</param>
        /// <returns>The account with the specified email address; or null if it does not exist.</returns>
        /// <exception cref="EmailNotRegisteredException">Thrown if the account could not be found.</exception>
        private async Task<Account> GetAccountByEmail(string email, bool throwIfNotFound = true)
        {
            var account = await _accountRepository.QueryFirstOrDefault(q => q
                .Where(a => string.Equals(a.EmailAddress, email, StringComparison.OrdinalIgnoreCase)));

            if (throwIfNotFound && account == null)
            {
                throw new EmailNotRegisteredException(email);
            }

            return account;
        }
    }
}