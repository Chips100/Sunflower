using Autofac;
using Sunflower.App.Server.Application.Authentication;

namespace Sunflower.App.Server.Application
{
    /// <summary>
    /// Registers types to be used to fulfill contracts of App-internal helpers.
    /// </summary>
    public class SunflowerAppModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterType<CookieAuthenticatorProvider>().As<IAuthenticatorProvider>();
        }
    }
}