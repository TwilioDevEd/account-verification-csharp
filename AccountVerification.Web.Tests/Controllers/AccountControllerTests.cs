using System.IO;
using System.Security.Principal;
using System.Threading.Tasks;
using System.Web;
using AccountVerification.Web.Controllers;
using AccountVerification.Web.Models;
using AccountVerification.Web.Tests.Common;
using Microsoft.AspNet.Identity;
using Microsoft.Owin.Security;
using Moq;
using NUnit.Framework;
using TestStack.FluentMVCTesting;

namespace AccountVerification.Web.Tests.Controllers
{
    /// <summary>
    /// Summary description for AccountControllerTests
    /// </summary>
    [TestFixture]
    public class AccountControllerTests: BaseTests
    {
        [SetUp]
        public void Setup()
        {
            InternalSetup();
        }

        [Test]
        public void GivenAVerifyRegistrationCodeAction_ThenRendersTheDefaultView()
        {
            // Arrange
            var controller = new AccountController(CurrentUserManagerMock.Object, CurrentSignInManagerMock.Object);

            // Act 
            controller.WithCallTo(c => c.VerifyRegistrationCode(ApplicationMessages.VerificationCodeSent))
                // Assert
                .ShouldRenderDefaultView();
        }

        [Test]
        public void GivenAResendVerificationCodeAction_ThenRendersTheDefaultView()
        {
            // Arrange
            var controller = new AccountController(CurrentUserManagerMock.Object, CurrentSignInManagerMock.Object);

            // Act 
            controller.WithCallTo(c => c.ResendVerificationCode(ApplicationMessages.VerificationCodeResent))
                // Assert
                .ShouldRenderDefaultView();
        }

        [Test]
        public void GivenAStatusAction_ThenRendersTheDefaultView()
        {
            // Arrange
            var controller = new AccountController(CurrentUserManagerMock.Object, CurrentSignInManagerMock.Object);
            // Act 
            controller.WithCallTo(c => c.Status())
                // Assert
                .ShouldRenderDefaultView();
        }
        
        [Test]
        public void GivenAResendVerificationCodeAction_WhenTheModelStateIsValid_ThenItRedirectsToVerifyRegsitrationCode()
        {
            // Arrange
            var controller = new AccountController(CurrentUserManagerMock.Object, CurrentSignInManagerMock.Object);
            // Act 
            var model = new ResendVerifyCodeViewModel()
            {
                Email = "user@email.com",
            };
            controller.WithCallTo(c => c.ResendVerificationCode(model))
                // Assert
                .ShouldRedirectTo<AccountController>(c => c.VerifyRegistrationCode(new VerifyCodeViewModel()));
        }

        [Test]
        public void GivenAResendVerificationCodeAction_WhenTheModelStateIsNotValid_ThenRendersTheDefaultView()
        {
            // Arrange
            var controller = new AccountController(CurrentUserManagerMock.Object, CurrentSignInManagerMock.Object);
            // Act 
            var model = new ResendVerifyCodeViewModel();
            controller.ModelState.AddModelError("", "Some kind of error");
            controller.WithCallTo(c => c.ResendVerificationCode(model))
                // Assert
                .ShouldRenderDefaultView();
        }

        [Test]
        public void GivenAResendVerificationCodeAction_WhenAnonymousUser_ThenRendersTheDefaultView()
        {
            // Arrange
            var controller = new AccountController(CurrentUserManagerMock.Object, CurrentSignInManagerMock.Object);
            // Act 
            controller.WithCallTo(c => c.ResendVerificationCode("user@email.com"))
                // Assert
                .ShouldRenderDefaultView();
        }

        [Test]
        public void GivenAVerifyRegistrationCodeAction_WhenAnonymousUser_ThenRendersTheDefaultView()
        {
            // Arrange
            var controller = new AccountController(CurrentUserManagerMock.Object, CurrentSignInManagerMock.Object);
            // Act 
            controller.WithCallTo(c => c.VerifyRegistrationCode(ApplicationMessages.VerificationCodeSent))
                // Assert
                .ShouldRenderDefaultView();
        }
    }
}
