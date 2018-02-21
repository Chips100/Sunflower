using Autofac;
using Sunflower.Business.Contracts;

namespace Sunflower.Business.Autofac
{
    /// <summary>
    /// Registers types to be used to fulfill contracts defined by Business.Contracts.
    /// </summary>
    public class SunflowerBusinessModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterType<AccountService>().As<IAccountService>();
            builder.RegisterType<StorageCreator>().As<IStorageCreator>();
        }
    }
}