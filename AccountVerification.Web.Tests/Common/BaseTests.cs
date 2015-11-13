using System.IO;
using System.Security.Principal;
using System.Threading.Tasks;
using System.Web;
using AccountVerification.Web.Models;
using Microsoft.AspNet.Identity;
using Microsoft.Owin.Security;
using Moq;

namespace AccountVerification.Web.Tests.Common
{
    public class BaseTests
    {
        protected const string VALID_CODE = "valid";
        protected const string UserId = "15e8cda1-3ea3-473d-902f-b62aa55816db";

        public Mock<ApplicationSignInManager> CurrentSignInManagerMock;
        public Mock<ApplicationUserManager> CurrentUserManagerMock;

        protected void InternalSetup()
        {
            HttpContext.Current = CreateHttpContext(userLoggedIn: false);
            var testStore = new TestUserStore();

            CurrentUserManagerMock = new Mock<ApplicationUserManager>(testStore);
            CurrentUserManagerMock.Setup(x => x.FindByEmailAsync(It.IsAny<string>()))
                .Returns(Task.FromResult(testStore.FindByIdAsync(UserId).Result));

            CurrentUserManagerMock.Setup(x => x.CreateAsync(It.IsAny<ApplicationUser>()))
                .Returns(Task.FromResult(IdentityResult.Success));

            CurrentUserManagerMock.Setup(u => u.RequestPhoneNumberConfirmationTokenAsync(It.IsAny<string>()))
                .Returns(Task.FromResult(0));

            CurrentUserManagerMock.Setup(u => u.ConfirmPhoneNumberAsync(It.IsAny<string>(), It.IsAny<string>()))
                .Returns(Task.FromResult(IdentityResult.Failed(ApplicationMessages.InvalidVerificationCode)));

            CurrentUserManagerMock.Setup(u => u.ConfirmPhoneNumberAsync(UserId, VALID_CODE))
                .Returns(Task.FromResult(IdentityResult.Success))
                .Callback(() => testStore.FindByIdAsync(UserId).Result.PhoneNumberConfirmed = true);

            CurrentUserManagerMock.Setup(u => u.FindByIdAsync(UserId))
                .Returns(Task.FromResult(testStore.FindByIdAsync(UserId).Result));

            var authenticationManager = new Mock<IAuthenticationManager>();
            CurrentSignInManagerMock =
                new Mock<ApplicationSignInManager>(CurrentUserManagerMock.Object, authenticationManager.Object);
        }

        protected static HttpContext CreateHttpContext(bool userLoggedIn)
        {
            var httpContext = new HttpContext(
                new HttpRequest(string.Empty, "http://localhost:25451/", string.Empty),
                new HttpResponse(new StringWriter())
                )
            {
                User = userLoggedIn
                    ? new GenericPrincipal(new GenericIdentity("userName"), new string[0])
                    : new GenericPrincipal(new GenericIdentity(string.Empty), new string[0])
            };

            return httpContext;
        }
    }
}