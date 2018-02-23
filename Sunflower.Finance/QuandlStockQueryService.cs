using Sunflower.Entities;
using Sunflower.Finance.Contracts;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sunflower.Finance
{
    /// <summary>
    /// Runs queries against a list of currently available stocks
    /// taken from Quandl (Stuttgart Börse Database).
    /// </summary>
    public class QuandlStockQueryService : IStockQueryService
    {
        private readonly QuandlCodesReader _quandlCodesReader = new QuandlCodesReader();

        /// <summary>
        /// Returns a complete list of all currently available stocks.
        /// </summary>
        /// <returns>A task that will complete with the list of stocks.</returns>
        public async Task<IEnumerable<Stock>> QueryAll()
        {
            // For now, take list from a provided file in the file system.
            var quandlCodeListFileName = @"SSE-datasets-codes.csv";

            // Create StreamReader from local file.
            using (var fileStream = File.OpenRead(quandlCodeListFileName))
            using (var streamReader = new StreamReader(fileStream, Encoding.UTF8))
            {
                // Parse file contents.
                var itemsFromFile = await _quandlCodesReader.Read(streamReader);

                return itemsFromFile
                    // Eliminate duplicate ISIN entries.
                    .GroupBy(x => x.Isin)
                    .Select(g => g.First())
                    // Convert to Sunflower Stock Entities.
                    .Select(x => new Stock
                    {
                        Isin = x.Isin,
                        Name = x.Name
                    });
            }
        }
    }
}
