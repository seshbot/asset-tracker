using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(KbcKegs.Startup))]
namespace KbcKegs
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
