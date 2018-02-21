using Microsoft.VisualStudio.TestTools.UnitTesting;
using Sunflower.Business.Exceptions;
using Sunflower.Business.Security;
using Sunflower.Business.Tests.Mocks;
using Sunflower.Entities;
using System.Linq;
using System.Threading.Tasks;

namespace Sunflower.Business.Tests
{
    /// <summary>
    /// Tests for correct behaviour of the AccountService.
    /// </summary>
    [TestClass]
    public class AccountServiceTests
    {
        /// <summary>
        /// Tests the creation of a new account.
        /// </summary>
        [TestMethod]
        public async Task AccountService_CreateAccount_CreatesAccount()
        {
            var accountRepository = new EntityRepositoryMock<Account>();
            var sut = new AccountService(accountRepository);

            await sut.CreateAccount("test@test.de", "testpassword");

            Assert.AreEqual(1, accountRepository.Entities.Count, "Unexpected number of accounts.");
            Assert.AreEqual("test@test.de", accountRepository.Entities.Values.First().EmailAddress, "Unexpected email address.");
        }

        /// <summary>
        /// Tests the correct generation of a pair 
        /// of hash and salt from the specified password.
        /// </summary>
        [TestMethod]
        public async Task AccountService_CreateAccount_GeneratesPassword()
        {
            var accountRepository = new EntityRepositoryMock<Account>();
            var sut = new AccountService(accountRepository);

            await sut.CreateAccount("test@test.de", "testpassword");

            var account = accountRepository.Entities.Values.First();
            var hashedPassword = new HashedPassword(account.PasswordHash, account.PasswordSalt);

            Assert.IsTrue(hashedPassword.EqualsPlainPassword("testpassword"), "Password not set correctly.");
        }

        /// <summary>
        /// Tests that the appropriate exception is thrown
        /// when trying to create an account with an email address
        /// that is already in use.
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(EmailAlreadyRegisteredException))]
        public async Task AccountService_CreateAccount_ThrowsEmailAlreadyRegisteredException()
        {
            var accountRepository = new EntityRepositoryMock<Account>(new Account
            {
                Id = 1,
                EmailAddress = "test@test.de"
            });

            var sut = new AccountService(accountRepository);
            await sut.CreateAccount("test@test.de", "password");
        }

        /// <summary>
        /// Tests changing a password with the AccountService.
        /// </summary>
        [TestMethod]
        public async Task AccountService_ChangePassword_ChangesPassword()
        {
            var accountRepository = new EntityRepositoryMock<Account>(new Account
            {
                Id = 1,
                EmailAddress = "test@test.de"
            });

            var sut = new AccountService(accountRepository);
            await sut.ChangePassword("test@test.de", "testpassword");

            var account = accountRepository.Entities.Values.First();
            var hashedPassword = new HashedPassword(account.PasswordHash, account.PasswordSalt);

            Assert.IsNotNull(account.PasswordHash, "account.PasswordHash");
            Assert.IsNotNull(account.PasswordSalt, "account.PasswordSalt");
            Assert.IsTrue(hashedPassword.EqualsPlainPassword("testpassword"), "Password not set correctly.");
        }

        /// <summary>
        /// Tests that the appropriate exception is thrown when trying
        /// to change the password of an account with an email address
        /// that is not registered.
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(EmailNotRegisteredException))]
        public async Task AccountService_ChangePassword_ThrowsEmailNotRegisteredException()
        {
            var accountRepository = new EntityRepositoryMock<Account>();

            var sut = new AccountService(accountRepository);
            await sut.ChangePassword("test@test.de", "testpassword");
        }

        /// <summary>
        /// Tests if the AccountService correctly checks a given password
        /// against a stored pair of hash and salt.
        /// </summary>
        [TestMethod]
        public async Task AccountService_CheckPassword_Success()
        {
            var hashedPassword = HashedPassword.CreateFromPlainPassword("testpassword");
            var accountRepository = new EntityRepositoryMock<Account>(new Account
            {
                Id = 1,
                EmailAddress = "test@test.de",
                PasswordHash = hashedPassword.Hash,
                PasswordSalt = hashedPassword.Salt
            });

            var sut = new AccountService(accountRepository);
            var result = await sut.CheckAccountPassword("test@test.de", "testpassword");

            Assert.AreEqual(true, result, "CheckPassword did not confirm correctness.");
        }

        /// <summary>
        /// Negative test for checking a password.
        /// </summary>
        [TestMethod]
        public async Task AccountService_CheckPassword_Fail()
        {
            var hashedPassword = HashedPassword.CreateFromPlainPassword("testpassword");
            var accountRepository = new EntityRepositoryMock<Account>(new Account
            {
                Id = 1,
                EmailAddress = "test@test.de",
                PasswordHash = hashedPassword.Hash,
                PasswordSalt = hashedPassword.Salt
            });

            var sut = new AccountService(accountRepository);
            var result = await sut.CheckAccountPassword("test@test.de", "somethingelse");

            Assert.AreEqual(false, result, "CheckPassword confirmed correctness.");
        }

        /// <summary>
        /// Tests that a password check for a non-existing account
        /// is treated like a normal faile check.
        /// </summary>
        [TestMethod]
        public async Task AccountService_CheckPassword_Fail_NotExists()
        {
            var hashedPassword = HashedPassword.CreateFromPlainPassword("testpassword");
            var accountRepository = new EntityRepositoryMock<Account>();

            var sut = new AccountService(accountRepository);
            var result = await sut.CheckAccountPassword("test@test.de", "somethingelse");

            Assert.AreEqual(false, result, "CheckPassword confirmed correctness.");
        }
    }
}