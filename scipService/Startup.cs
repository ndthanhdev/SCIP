using Microsoft.Owin;
using Owin;

[assembly: OwinStartup(typeof(scipService.Startup))]

namespace scipService
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureMobileApp(app);
        }
    }
}