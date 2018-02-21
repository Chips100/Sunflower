using Microsoft.VisualStudio.TestTools.UnitTesting;
using Sunflower.App.Server.Controllers;
using Sunflower.App.Server.Tests.Mocks;
using System.Threading.Tasks;

namespace Sunflower.App.Server.Tests.Controllers.api
{
    /// <summary>
    /// Tests for the AccountController.
    /// </summary>
    [TestClass]
    public class AccountControllerTests
    {
        /// <summary>
        /// Tests the creation of a new account.
        /// </summary>
        [TestMethod]
        public async Task AccountController_Register_CreatesAccount()
        {
            var email = "test@test.de";
            var password = "testpassword";

            var accountService = new AccountServiceMock();
            var sut = new AccountController(accountService, new AuthenticatorProviderMock());

            await sut.Register(email, password);

            Assert.AreEqual(email, accountService.CreatedAccountEmail, "Unexpected Email.");
            Assert.AreEqual(password, accountService.CreatedAccountPassword, "Unexpected Password");
        }

        /// <summary>
        /// Tests the process of logging in.
        /// </summary>
        [TestMethod]
        public async Task AccountController_Login_Success()
        {
            var email = "test@test.de";
            var accountService = new AccountServiceMock() { CheckAccountPasswordResult = true };
            var authenticatorProvider = new AuthenticatorProviderMock();
            var sut = new AccountController(accountService, authenticatorProvider);

            var result = await sut.Login(email, "testpassword");

            Assert.IsTrue(result, "result");
            Assert.IsTrue(authenticatorProvider.IsAuthenticated, "IsAuthenticated");
            Assert.AreEqual(email, authenticatorProvider.Email, "Email");
        }

        /// <summary>
        /// Tests the correct behaviour when trying to login with a wrong password.
        /// </summary>
        [TestMethod]
        public async Task AccountController_Login_Fail()
        {
            var email = "test@test.de";
            var accountService = new AccountServiceMock() { CheckAccountPasswordResult = false };
            var authenticatorProvider = new AuthenticatorProviderMock();
            var sut = new AccountController(accountService, authenticatorProvider);

            var result = await sut.Login(email, "testpassword");

            Assert.IsFalse(result, "result");
            Assert.IsFalse(authenticatorProvider.IsAuthenticated, "IsAuthenticated");
            Assert.IsNull(authenticatorProvider.Email, "Email");
        }

        /// <summary>
        /// Tests the correct behaviour for changing the password.
        /// </summary>
        [TestMethod]
        public async Task AccountController_ChangePassword_Success()
        {
            var email = "test@test.de";
            var password = "testpassword";
            var accountService = new AccountServiceMock() { CheckAccountPasswordResult = true };
            var authenticatorProvider = new AuthenticatorProviderMock() { Email = email };
            var sut = new AccountController(accountService, authenticatorProvider);

            var result = await sut.ChangePassword("oldPassword", password);

            Assert.IsTrue(result, "result");
            Assert.AreEqual(email, accountService.ChangedEmail, "accountService.ChangedEmail");
            Assert.AreEqual(password, accountService.ChangedPassword, "accountService.ChangedPassword");
        }

        /// <summary>
        /// Tests the correct behaviour of the AccountController
        /// when trying to change a password with the wrong old password.
        /// </summary>
        [TestMethod]
        public async Task AccountController_ChangePassword_Fail_WrongPassword()
        {
            var email = "test@test.de";
            var password = "testpassword";
            var accountService = new AccountServiceMock() { CheckAccountPasswordResult = false };
            var authenticatorProvider = new AuthenticatorProviderMock() { Email = email };
            var sut = new AccountController(accountService, authenticatorProvider);

            var result = await sut.ChangePassword("oldPassword", password);

            Assert.IsFalse(result, "result");
            Assert.IsNull(accountService.ChangedEmail, "accountService.ChangedEmail");
            Assert.IsNull(accountService.ChangedPassword, "accountService.ChangedPassword");
        }
    }
}