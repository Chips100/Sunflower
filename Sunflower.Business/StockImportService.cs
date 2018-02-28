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
        private readonly IEntityQuerySource _entityQuerySource;
        private readonly IEntityRepositoryFactory _entityRepositoryFactory;
        private readonly IStockQueryService _stockQueryService;

        /// <summary>
        /// Creates a StockImportService.
        /// </summary>
        /// <param name="entityQuerySource">EntityQuerySource to run queries against the persistent storage.</param>
        /// <param name="entityRepositoryFactory">EntityRepositoryFactory to make changes to the persistent storage.</param>
        /// <param name="stockQueryService">Service to query the current list of available stocks.</param>
        public StockImportService(IEntityQuerySource entityQuerySource, IEntityRepositoryFactory entityRepositoryFactory, IStockQueryService stockQueryService)
        {
            _entityQuerySource = entityQuerySource;
            _entityRepositoryFactory = entityRepositoryFactory;
            _stockQueryService = stockQueryService;
        }

        /// <summary>
        /// Imports the list of currently available stocks.
        /// </summary>
        /// <returns>A task that will complete when the import has completed.</returns>
        public async Task ImportStocks()
        {
            // Collect both list of stocks: stored and currently available.
            var storedStocks = (await _entityQuerySource.Query(q => q.Get<Stock>())).ToDictionary(x => x.Isin);
            var currentStocks = await _stockQueryService.QueryAll();

            await _entityRepositoryFactory.Use(async repository =>
            {
                foreach (var stock in currentStocks)
                {
                    // Create stock in storage if not yet present.
                    if (!storedStocks.TryGetValue(stock.Isin, out Stock matchingStoredStock))
                    {
                        repository.Add(stock);
                    }
                    // Update name if this has changed.
                    else if (stock.Name != matchingStoredStock.Name)
                    {
                        await repository.Change<Stock>(matchingStoredStock.Id, s =>
                        {
                            s.Name = stock.Name;
                        });
                    }
                }

                // For now, do not delete obsolete stocks...
            });
        }
    }
}