using Sunflower.Data.Contracts;
using Sunflower.Entities;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sunflower.Business.Setup
{
    /// <summary>
    /// Fills the list of stocks in the persistent storage with initial values.
    /// </summary>
    public sealed class StockListInitializer
    {
        private IEntityRepository<Stock> _stockRepository;

        public StockListInitializer(IEntityRepository<Stock> stockRepository)
        {
            _stockRepository = stockRepository;
        }

        public async Task InitializeStockList()
        {
            var existingIsins = new HashSet<string>(await _stockRepository.Query(q => q.Select(x => x.Isin)));

            foreach(var stock in ReadStockList().Where(s => !existingIsins.Contains(s.Isin)))
            {
                await _stockRepository.Create(stock);
            }
        }
        
        private IEnumerable<Stock> ReadStockList()
        {
            var assembly = typeof(StockListInitializer).Assembly;
            var ns = typeof(StockListInitializer).Namespace;

            using (var stream = assembly.GetManifestResourceStream($"{ns}.StockList.csv"))
            {
                using (var streamReader = new StreamReader(stream, Encoding.UTF8))
                {
                    string line;
                    while((line = streamReader.ReadLine()) != null)
                    {
                        var parts = line.Split(';');
                        yield return new Stock
                        {
                            Isin = parts[0],
                            Name = parts[1]
                        };
                    }
                }
            }
        }
    }
}
