using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(Castalia.WEB.Startup))]
namespace Castalia.WEB
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
