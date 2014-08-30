using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(HSM.Startup))]
namespace HSM
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
