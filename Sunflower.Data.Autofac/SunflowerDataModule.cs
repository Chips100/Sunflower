using Autofac;
using Sunflower.Data.Contracts;

namespace Sunflower.Data.Autofac
{
    /// <summary>
    /// Registers types to be used to fulfill contracts defined by Data.Contracts.
    /// </summary>
    public class SunflowerDataModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterType<EntityQuerySource>().As<IEntityQuerySource>().SingleInstance();
            builder.RegisterType<EntityRepositoryFactory>().As<IEntityRepositoryFactory>().SingleInstance();
            builder.RegisterType<SunflowerStorageCreator>().As<IStorageCreator>().SingleInstance();
        }
    }
}