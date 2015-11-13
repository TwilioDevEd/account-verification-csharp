using AccountVerification.Web.Tests.Common;
using NUnit.Framework;

namespace AccountVerification.Web.Tests.Identity
{
    /// <summary>
    /// Summary description for ApplicationUserManagerTests
    /// </summary>
    [TestFixture]
    public class ApplicationUserManagerTests: BaseTests
    {
        [SetUp]
        public void Setup()
        {
            InternalSetup();
        }

        [Test]
        public void GivenAnUnVerifiedUserToVerifyPhone_WithValidCode_ThenVerifyIt()
        {
            // Arrange
            var userManager = CurrentUserManagerMock.Object;

            // Act 
            var confirmation =
                userManager.ConfirmPhoneNumberAsync(UserId, VALID_CODE).Result;
            // Assert
            Assert.That(confirmation.Succeeded, Is.EqualTo(true));
            Assert.That(userManager.FindByIdAsync(UserId).Result.PhoneNumberConfirmed, Is.EqualTo(true));
        }

        [Test]
        public void GivenAnUnVerifiedUserToVerifyPhone_WithInvalidCode_ThenDontVerifyIt()
        {
            // Arrange
            var userManager = CurrentUserManagerMock.Object;
            var userId = "15e8cda1-3ea3-473d-902f-b62aa55816db";

            // Act 
            var confirmation =
                userManager.ConfirmPhoneNumberAsync(userId, "any code than the valid").Result;
            // Assert
            Assert.That(confirmation.Succeeded, Is.EqualTo(false));
            Assert.That(userManager.FindByIdAsync(UserId).Result.PhoneNumberConfirmed, Is.EqualTo(false));
        }
    }
}
