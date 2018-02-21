using Autofac;
using Sunflower.Data.Contracts;
using Sunflower.Entities;

namespace Sunflower.Data.Autofac
{
    /// <summary>
    /// Registers types to be used to fulfill contracts defined by Data.Contracts.
    /// </summary>
    public class SunflowerDataModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterType<EntityRepository<Account>>().As<IEntityRepository<Account>>();
            builder.RegisterType<SunflowerStorageCreator>().As<IStorageCreator>();
        }
    }
}