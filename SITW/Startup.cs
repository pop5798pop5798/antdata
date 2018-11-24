using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(SITW.Startup))]
namespace SITW
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
            app.MapSignalR();
        }
    }
}
