using Sunflower.Entities;
using Sunflower.Finance.Contracts;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Sunflower.Finance
{
    /// <summary>
    /// Runs queries against a list of currently available stocks
    /// taken from Quandl (Stuttgart Börse Database).
    /// </summary>
    public class QuandlStockQueryService : IStockQueryService
    {
        private QuandlCodesProvider _quandlCodesProvider;

        public QuandlStockQueryService(QuandlCodesProvider quandlCodesProvider)
        {
            _quandlCodesProvider = quandlCodesProvider;
        }

        /// <summary>
        /// Returns a complete list of all currently available stocks.
        /// </summary>
        /// <returns>A task that will complete with the list of stocks.</returns>
        public async Task<IEnumerable<Stock>> QueryAll()
        {
            return _quandlCodesProvider.Values.Select(x => new Stock
            {
                Isin = x.Isin,
                Name = x.Name
            });
        }
    }
}
