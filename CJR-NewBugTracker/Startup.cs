using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(CJR_NewBugTracker.Startup))]
namespace CJR_NewBugTracker
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
