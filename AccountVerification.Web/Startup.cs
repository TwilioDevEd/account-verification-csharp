using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(AccountVerification.Web.Startup))]
namespace AccountVerification.Web
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
