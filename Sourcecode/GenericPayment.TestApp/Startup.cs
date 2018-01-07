using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(GenericPayment.TestApp.Startup))]
namespace GenericPayment.TestApp
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
