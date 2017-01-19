using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(Forex.Mvc.Startup))]
namespace Forex.Mvc
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
