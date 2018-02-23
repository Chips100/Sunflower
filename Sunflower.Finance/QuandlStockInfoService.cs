using Sunflower.Entities;
using Sunflower.Finance.Contracts;
using System;
using System.Threading.Tasks;

namespace Sunflower.Finance
{
    /// <summary>
    /// Provides access to real time information about individual stocks
    /// from Quandl WebService data.
    /// </summary>
    public sealed class QuandlStockInfoService : IStockInfoService
    {
        /// <summary>
        /// Gets the current value per share of the specified stock.
        /// </summary>
        /// <param name="stock">Stock for which the current value per share should be returned.</param>
        /// <returns>A task that will complete with the current value per share.</returns>
        public Task<decimal> GetCurrentShareValue(Stock stock)
        {
            throw new NotImplementedException();
        }
    }
}
