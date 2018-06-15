using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(VendorManagementSystem.Startup))]
[assembly: log4net.Config.XmlConfigurator(ConfigFile = "Web.config", Watch = true)]
namespace VendorManagementSystem
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            app.MapSignalR();
            ConfigureAuth(app);
        }
    }
}
