using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(FactOrFictionWeb.Startup))]
namespace FactOrFictionWeb
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
