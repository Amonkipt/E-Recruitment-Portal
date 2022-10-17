using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(E_Recruitment_Portal.Startup))]
namespace E_Recruitment_Portal
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
