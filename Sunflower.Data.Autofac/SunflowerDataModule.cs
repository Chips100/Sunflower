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
            builder.RegisterGeneric(typeof(EntityRepository<>)).As(typeof(IEntityRepository<>));
            builder.RegisterType<SunflowerStorageCreator>().As<IStorageCreator>();
        }
    }
}