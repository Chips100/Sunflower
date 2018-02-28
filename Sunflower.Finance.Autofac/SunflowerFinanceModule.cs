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
            builder.RegisterType<QuandlStockInfoService>().As<IStockInfoService>().SingleInstance();
            builder.RegisterType<QuandlStockQueryService>().As<IStockQueryService>().SingleInstance();
            builder.RegisterType<QuandlCodesProvider>().SingleInstance().SingleInstance();
        }
    }
}