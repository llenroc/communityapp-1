using Microsoft.Owin;
using Owin;
using TechReady.Portal;

[assembly: OwinStartup(typeof(Startup))]
namespace TechReady.Portal
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
