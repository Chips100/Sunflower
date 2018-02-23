using Microsoft.Extensions.Configuration;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Sunflower.Finance
{
    /// <summary>
    /// Provides access to the database entries stored in the Quandl code file.
    /// </summary>
    /// <remarks>
    /// The Quandl code file is a file with a list of all
    /// databases from the "Börse Stuttgart" data product.
    /// Each database holds values for a single stock.
    /// To update the file, download it from:
    /// https://www.quandl.com/api/v3/databases/SSE/codes.json
    /// </remarks>
    public sealed class QuandlCodesProvider
    {
        private readonly QuandlCodeFileReader _quandlCodesReader = new QuandlCodeFileReader();
        private readonly string _quandlCodeFilePath;

        /// <summary>
        /// Creates a QuandlCodesProvider.
        /// </summary>
        /// <param name="configuration">Configuration with the file path to the code file.</param>
        public QuandlCodesProvider(IConfiguration configuration)
        {
            _quandlCodeFilePath = configuration["QuandlCodeFilePath"];

            Values = ReadAllItems();
        }

        /// <summary>
        /// Values found in the Quandl code file.
        /// </summary>
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
                return _quandlCodesReader.Read(streamReader);
            }
        }
    }
}