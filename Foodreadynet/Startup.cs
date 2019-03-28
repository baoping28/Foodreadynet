using Owin;
using Microsoft.Owin;
[assembly: OwinStartup(typeof(IdentitySample.Startup))]
namespace IdentitySample
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
