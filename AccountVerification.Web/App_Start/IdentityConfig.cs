using System;
using System.Diagnostics;
using System.Globalization;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin;
using Microsoft.Owin.Security;
using AccountVerification.Web.Models;
using Authy.Net;
using Twilio;
using Twilio.Clients;
using Twilio.Rest.Api.V2010.Account;
using Twilio.Types;

namespace AccountVerification.Web
{
    public class SmsService : IIdentityMessageService
    {
        public SmsService()
        {
            TwilioClient.Init(TwilioSettings.AccountSID, TwilioSettings.AuthToken);
        }

        public SmsService(ITwilioRestClient twilioRestClient) : this()
        {
            TwilioClient.SetRestClient(twilioRestClient);
        }

        public async Task SendAsync(IdentityMessage message)
        {
            var to = new PhoneNumber(message.Destination);
            await MessageResource.CreateAsync(
                to,
                from: new PhoneNumber(TwilioSettings.PhoneNumber),
                body: message.Body);
        }
    }

    public class EmailService : IIdentityMessageService
    {
        public Task SendAsync(IdentityMessage message)
        {
            // Plug in your email service here to send an email.
            return Task.FromResult(0);
        }
    }

    public interface IApplicationUserManager
    {
        Task<IdentityResult> ConfirmPhoneNumberAsync(string userId, string code);
        Task RequestPhoneNumberConfirmationTokenAsync(string userId);
    }

    // Configure the application user manager used in this application. UserManager is defined in 
    // ASP.NET Identity and is used by the application.
    public class ApplicationUserManager : UserManager<ApplicationUser>, IApplicationUserManager
    {
        private AuthyClient _authyApiClient;

        public ApplicationUserManager(IUserStore<ApplicationUser> store)
            : base(store)
        {
        }

        public static ApplicationUserManager Create(IdentityFactoryOptions<ApplicationUserManager> options, IOwinContext context)
        {
            var manager = new ApplicationUserManager(new UserStore<ApplicationUser>(context.Get<ApplicationDbContext>()));
            // Configure validation logic for usernames
            manager.UserValidator = new UserValidator<ApplicationUser>(manager)
            {
                AllowOnlyAlphanumericUserNames = false,
                RequireUniqueEmail = true
            };

            // Configure validation logic for passwords
            manager.PasswordValidator = new PasswordValidator
            {
                RequiredLength = 6,
                RequireNonLetterOrDigit = false,
                RequireDigit = false,
                RequireLowercase = true,
                RequireUppercase = false,
            };

            // Configure user lockout defaults
            manager.UserLockoutEnabledByDefault = true;
            manager.DefaultAccountLockoutTimeSpan = TimeSpan.FromMinutes(5);
            manager.MaxFailedAccessAttemptsBeforeLockout = 5;

            // Register two factor authentication providers. This application uses Phone and Emails 
            // as a step of receiving a code for verifying the user
            // You can write your own provider and plug it in here.

            manager.RegisterTwoFactorProvider("PhoneCode", new PhoneNumberTokenProvider<ApplicationUser>
            {
                MessageFormat = "Your security code is: {0}"
            });
            manager.RegisterTwoFactorProvider("EmailCode", new EmailTokenProvider<ApplicationUser>
            {
                Subject = "SecurityCode",
                BodyFormat = "Your security code is {0}"
            });
            manager.EmailService = new EmailService();
            manager.SmsService = new SmsService();

            var dataProtectionProvider = options.DataProtectionProvider;
            if (dataProtectionProvider != null)
            {
                manager.UserTokenProvider =
                    new DataProtectorTokenProvider<ApplicationUser>(dataProtectionProvider.Create("ASP.NET Identity"));
            }
            return manager;
        }

        #region User Manager extended method interface
        public virtual async Task<IdentityResult> ConfirmPhoneNumberAsync(string userId, string code)
        {
            ApplicationUser user = await FindByIdAsync(userId);

            if (user == null)
                throw new InvalidOperationException(string.Format(CultureInfo.CurrentCulture, ApplicationMessages.UserIdNotFound));

            IdentityResult identityResult;

            if (!AuthyApiClient.VerifyToken(user.AuthyUserId, code).Success)
            {
                identityResult = IdentityResult.Failed(ApplicationMessages.InvalidVerificationCode);
            }
            else
            {
                IUserPhoneNumberStore<ApplicationUser, string> store = (IUserPhoneNumberStore<ApplicationUser, string>)Store;

                await store.SetPhoneNumberConfirmedAsync(user, true);
                identityResult = await UpdateAsync(user);
            }
            return identityResult;
        }

        public virtual async Task RequestPhoneNumberConfirmationTokenAsync(string userId)
        {
            ApplicationUser user = await FindByIdAsync(userId);

            if (user == null)
                throw new InvalidOperationException(string.Format(CultureInfo.CurrentCulture, ApplicationMessages.UserIdNotFound));

            if (String.IsNullOrEmpty(user.AuthyUserId))
            {
                await RegisterAsAuthyUser(user);
            }

            AuthyApiClient.SendSms(user.AuthyUserId, force:true);
        }

        #region Private Methods
        private async Task RegisterAsAuthyUser(ApplicationUser user)
        {
            string authyUserId = AuthyApiClient.RegisterUser(user.Email, user.PhoneNumber.Replace(String.Format("+{0}", user.CountryCode), string.Empty), Convert.ToInt32(user.CountryCode)).UserId;
            user.AuthyUserId = authyUserId;

            await UpdateAsync(user);
        }

        private AuthyClient AuthyApiClient
        {
            get { return _authyApiClient ?? (_authyApiClient = new AuthyClient(AuthySettings.Key, test: false)); }
        }
        #endregion 

        #endregion
    }

    // Configure the application sign-in manager which is used in this application.
    public class ApplicationSignInManager : SignInManager<ApplicationUser, string>
    {
        public ApplicationSignInManager(ApplicationUserManager userManager, IAuthenticationManager authenticationManager)
            : base(userManager, authenticationManager)
        {
        }

        public override Task<ClaimsIdentity> CreateUserIdentityAsync(ApplicationUser user)
        {
            return user.GenerateUserIdentityAsync((ApplicationUserManager)UserManager);
        }

        public static ApplicationSignInManager Create(IdentityFactoryOptions<ApplicationSignInManager> options, IOwinContext context)
        {
            return new ApplicationSignInManager(context.GetUserManager<ApplicationUserManager>(), context.Authentication);
        }
    }
}
