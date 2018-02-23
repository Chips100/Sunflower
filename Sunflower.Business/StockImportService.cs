using Sunflower.Business.Contracts;
using Sunflower.Data.Contracts;
using Sunflower.Entities;
using Sunflower.Finance.Contracts;
using System.Linq;
using System.Threading.Tasks;

namespace Sunflower.Business
{
    /// <summary>
    /// Allows importing the list of currently available stocks
    /// into the Sunflower storage for usage in the application.
    /// </summary>
    public sealed class StockImportService : IStockImportService
    {
        private readonly IStockQueryService _stockQueryService;
        private readonly IEntityRepository<Stock> _stockRepository;

        /// <summary>
        /// Creates a StockImportService.
        /// </summary>
        /// <param name="stockRepository">Repository to apply changes to the storage.</param>
        /// <param name="stockQueryService">Service to query the current list of available stocks.</param>
        public StockImportService(IEntityRepository<Stock> stockRepository, IStockQueryService stockQueryService)
        {
            _stockQueryService = stockQueryService;
            _stockRepository = stockRepository;
        }

        /// <summary>
        /// Imports the list of currently available stocks.
        /// </summary>
        /// <returns>A task that will complete when the import has completed.</returns>
        public async Task ImportStocks()
        {
            // Collect both list of stocks: stored and currently available.
            var storedStocks = (await _stockRepository.Query(q => q)).ToDictionary(x => x.Isin);
            var currentStocks = await _stockQueryService.QueryAll();

            foreach(var stock in currentStocks)
            {
                // Create stock in storage if not yet present.
                if (!storedStocks.TryGetValue(stock.Isin, out Stock matchingStoredStock))
                {
                    await _stockRepository.Create(stock);
                }
                // Update name if this has changed.
                else if (stock.Name != matchingStoredStock.Name)
                {
                    await _stockRepository.Change(matchingStoredStock.Id, s =>
                    {
                        s.Name = stock.Name;
                    });
                }
            }

            // For now, do not delete obsolete stocks...
        }
    }
}