using Sunflower.Entities;
using Sunflower.Finance.Contracts;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Sunflower.Finance
{
    /// <summary>
    /// Runs queries against a list of currently available stocks
    /// taken from Quandl (Stuttgart Börse data product).
    /// </summary>
    public class QuandlStockQueryService : IStockQueryService
    {
        private QuandlCodesProvider _quandlCodesProvider;

        /// <summary>
        /// Creates a QuandlStockQueryService.
        /// </summary>
        /// <param name="quandlCodesProvider">Underlying provider with list of available stock databases.</param>
        public QuandlStockQueryService(QuandlCodesProvider quandlCodesProvider)
        {
            _quandlCodesProvider = quandlCodesProvider;
        }

        /// <summary>
        /// Returns a complete list of all currently available stocks.
        /// </summary>
        /// <returns>A task that will complete with the list of stocks.</returns>
        public Task<IEnumerable<Stock>> QueryAll()
        {
            // Simply yield all stocks found in the Quandl code file.
            // These are all stocks for which a database exists.
            return Task.FromResult(_quandlCodesProvider.Values.Select(x => new Stock
            {
                Isin = x.Isin,
                Name = x.Name
            }));
        }
    }
}