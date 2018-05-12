using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(_15DH110184_HoangVi.Startup))]
namespace _15DH110184_HoangVi
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
