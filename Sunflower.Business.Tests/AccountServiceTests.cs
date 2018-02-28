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
            var repositoryMock = new EntityRepositoryMock();
            var sut = new AccountService(repositoryMock, repositoryMock);

            await sut.CreateAccount("test@test.de", "testpassword");

            var accountEntities = repositoryMock.GetEntities<Account>();
            Assert.AreEqual(1, accountEntities.Count, "Unexpected number of accounts.");
            Assert.AreEqual("test@test.de", accountEntities.First().EmailAddress, "Unexpected email address.");
        }

        /// <summary>
        /// Tests the correct generation of a pair 
        /// of hash and salt from the specified password.
        /// </summary>
        [TestMethod]
        public async Task AccountService_CreateAccount_GeneratesPassword()
        {
            var repositoryMock = new EntityRepositoryMock();
            var sut = new AccountService(repositoryMock, repositoryMock);

            await sut.CreateAccount("test@test.de", "testpassword");

            var account = repositoryMock.GetEntities<Account>().Single();
            var hashedPassword = new HashedPassword(account.PasswordHash, account.PasswordSalt);

            Assert.IsTrue(hashedPassword.EqualsPlainPassword("testpassword"), "Password not set correctly.");
        }
        
        /// <summary>
        /// Tests the creation of an initial transaction for the starting budget.
        /// </summary>
        [TestMethod]
        public async Task AccountService_CreateAccount_CreatesInitialTransaction()
        {
            var repositoryMock = new EntityRepositoryMock();
            var sut = new AccountService(repositoryMock, repositoryMock);

            await sut.CreateAccount("test@test.de", "testpassword");

            var account = repositoryMock.GetEntities<Account>().Single();
            Assert.AreEqual(1, account.ContextFreeTransactions.Count, "Count");

            var transaction = account.ContextFreeTransactions.Single();
            Assert.AreEqual(account.Id, transaction.AccountId, "AccountId");
            Assert.AreEqual(10000, transaction.Amount, "Amount");
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
            var repositoryMock = new EntityRepositoryMock();
            var sut = new AccountService(repositoryMock, repositoryMock);

            repositoryMock.Add(new Account
            {
                Id = 1,
                EmailAddress = "test@test.de"
            });
            
            await sut.CreateAccount("test@test.de", "password");
        }

        /// <summary>
        /// Tests changing a password with the AccountService.
        /// </summary>
        [TestMethod]
        public async Task AccountService_ChangePassword_ChangesPassword()
        {
            var repositoryMock = new EntityRepositoryMock();
            var sut = new AccountService(repositoryMock, repositoryMock);

            repositoryMock.Add(new Account
            {
                Id = 1,
                EmailAddress = "test@test.de"
            });
            
            await sut.ChangePassword(1, "testpassword");

            var account = repositoryMock.GetEntities<Account>().Single();
            var hashedPassword = new HashedPassword(account.PasswordHash, account.PasswordSalt);

            Assert.IsNotNull(account.PasswordHash, "account.PasswordHash");
            Assert.IsNotNull(account.PasswordSalt, "account.PasswordSalt");
            Assert.IsTrue(hashedPassword.EqualsPlainPassword("testpassword"), "Password not set correctly.");
        }

        /// <summary>
        /// Tests if the AccountService correctly checks a given password
        /// against a stored pair of hash and salt.
        /// </summary>
        [TestMethod]
        public async Task AccountService_CheckPassword_Success()
        {
            var repositoryMock = new EntityRepositoryMock();
            var sut = new AccountService(repositoryMock, repositoryMock);

            var hashedPassword = HashedPassword.CreateFromPlainPassword("testpassword");
            repositoryMock.Add(new Account
            {
                Id = 1,
                EmailAddress = "test@test.de",
                PasswordHash = hashedPassword.Hash,
                PasswordSalt = hashedPassword.Salt
            });

            var result = await sut.CheckAccountPassword("test@test.de", "testpassword");

            Assert.AreEqual(true, result, "CheckPassword did not confirm correctness.");
        }

        /// <summary>
        /// Negative test for checking a password.
        /// </summary>
        [TestMethod]
        public async Task AccountService_CheckPassword_Fail()
        {
            var repositoryMock = new EntityRepositoryMock();
            var sut = new AccountService(repositoryMock, repositoryMock);

            var hashedPassword = HashedPassword.CreateFromPlainPassword("testpassword");
            repositoryMock.Add(new Account
            {
                Id = 1,
                EmailAddress = "test@test.de",
                PasswordHash = hashedPassword.Hash,
                PasswordSalt = hashedPassword.Salt
            });

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
            var repositoryMock = new EntityRepositoryMock();
            var sut = new AccountService(repositoryMock, repositoryMock);

            var result = await sut.CheckAccountPassword("test@test.de", "somethingelse");

            Assert.AreEqual(false, result, "CheckPassword confirmed correctness.");
        }
    }
}