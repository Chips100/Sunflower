using Microsoft.Extensions.Configuration;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace Sunflower.Finance
{
    public sealed class QuandlCodesProvider
    {
        private readonly QuandlCodesReader _quandlCodesReader = new QuandlCodesReader();
        private readonly string _quandlCodeFilePath;

        public QuandlCodesProvider(IConfiguration configuration)
        {
            _quandlCodeFilePath = configuration["QuandlCodeFilePath"];

            Values = ReadAllItems();
        }

        public IEnumerable<QuandlCodeItem> Values { get; }

        /// <summary>
        /// Returns a complete list of all currently available stocks.
        /// </summary>
        /// <returns>A task that will complete with the list of stocks.</returns>
        private IEnumerable<QuandlCodeItem> ReadAllItems()
        {
            // Create StreamReader from local file.
            using (var fileStream = File.OpenRead(_quandlCodeFilePath))
            using (var streamReader = new StreamReader(fileStream, Encoding.UTF8))
            {
                // Parse file contents.
                var itemsFromFile = _quandlCodesReader.Read(streamReader);

                return itemsFromFile
                    // Eliminate duplicate ISIN entries.
                    .GroupBy(x => x.Isin)
                    .Select(g => g.First());
            }
        }
    }
}
