using System.Configuration;
using System.Web.Configuration;

namespace AccountVerification.Web
{
    public class TwilioSettings
    {
        public static string AccountSID
        {
            get { return ConfigurationManager.AppSettings["TwilioAccountSID"]; }
        }

        public static string AuthToken
        {
            get { return ConfigurationManager.AppSettings["TwilioAuthToken"]; }
        }

        public static string PhoneNumber
        {
            get { return ConfigurationManager.AppSettings["TwilioNumber"]; }
        }
    }

    public class AuthySettings
    {
        public static string Key
        {
            get { return ConfigurationManager.AppSettings["AuthyKey"]; }
        }
    }
}