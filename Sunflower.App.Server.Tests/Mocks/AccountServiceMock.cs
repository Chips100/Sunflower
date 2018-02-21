﻿using Sunflower.Business.Contracts;
using System.Threading.Tasks;

namespace Sunflower.App.Server.Tests.Mocks
{
    /// <summary>
    /// Mocks an AccountService, storing the invoked operations.
    /// </summary>
    public class AccountServiceMock : IAccountService
    {
        /// <summary>
        /// E-Mail address of the account that has been created with this AccountService.
        /// </summary>
        public string CreatedAccountEmail { get; private set; }

        /// <summary>
        /// Password of the account that has been created with this AccountService.
        /// </summary>
        public string CreatedAccountPassword { get; private set; }

        /// <summary>
        /// E-Mail address of the account of which the password has been changed with this AccountService.
        /// </summary>
        public string ChangedEmail { get; private set; }

        /// <summary>
        /// New Password of the account of which the password has been changed with this AccountService.
        /// </summary>
        public string ChangedPassword { get; private set; }

        /// <summary>
        /// Result that should be returned when performing a password check with this AccountService.
        /// </summary>
        public bool CheckAccountPasswordResult { get; set; }

        /// <summary>
        /// Changes the password of the specified account.
        /// </summary>
        /// <param name="email">E-Mail Address of the account of which to change the password.</param>
        /// <param name="newPassword">Password that should be used for the account.</param>
        /// <returns>A Task that will complete when the password has been changed.</returns>
        public Task ChangePassword(string email, string newPassword)
        {
            ChangedEmail = email;
            ChangedPassword = newPassword;
            return Task.FromResult(0);
        }

        /// <summary>
        /// Checks a password for the specified account.
        /// </summary>
        /// <param name="email">E-Mail Address of the account to check the password for.</param>
        /// <param name="password">Password that should match the password of the account.</param>
        /// <returns>True, if the password matches the password of the account; otherwise false.</returns>
        public Task<bool> CheckAccountPassword(string email, string password)
        {
            return Task.FromResult(CheckAccountPasswordResult);
        }

        /// <summary>
        /// Creates a new account.
        /// </summary>
        /// <param name="email">E-Mail Address that the account should be registered with.</param>
        /// <param name="password">Password of the account.</param>
        /// <returns>A Task that will complete when the account has been created.</returns>
        public Task CreateAccount(string email, string password)
        {
            CreatedAccountEmail = email;
            CreatedAccountPassword = password;
            return Task.FromResult(0);
        }
    }
}