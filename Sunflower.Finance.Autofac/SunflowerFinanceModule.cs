using Autofac;
using Sunflower.Finance.Contracts;

namespace Sunflower.Finance.Autofac
{
    /// <summary>
    /// Registers types to be used to fulfill contracts defined by Finance.Contracts.
    /// </summary>
    public sealed class SunflowerFinanceModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterType<QuandlStockInfoService>().As<IStockInfoService>();
            builder.RegisterType<QuandlStockQueryService>().As<IStockQueryService>();
        }
    }
}
