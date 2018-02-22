using Sunflower.Finance.Contracts;
using System;
using System.Threading.Tasks;

namespace Sunflower.Finance
{
    public sealed class StockInfoService : IStockInfoService
    {
        public Task<decimal> GetCurrentShareValue(string isin)
        {
            throw new NotImplementedException();
        }
    }
}
