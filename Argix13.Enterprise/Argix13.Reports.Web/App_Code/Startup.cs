using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(Argix.Startup))]
namespace Argix {
    public partial class Startup {
        public void Configuration(IAppBuilder app) {
            ConfigureAuth(app);
        }
    }
}
