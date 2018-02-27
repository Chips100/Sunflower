using Microsoft.Extensions.Configuration;
using Sunflower.Entities;
using Sunflower.Finance.Contracts;
using System;
using System.Globalization;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace Sunflower.Finance
{
    /// <summary>
    /// Provides access to real time information about individual stocks
    /// from Quandl WebService data.
    /// </summary>
    public sealed class QuandlStockInfoService : IStockInfoService
    {
        /// <summary>
        /// Name of the column which holds the closing value we are interested in.
        /// </summary>
        private const string LastColumnName = "Last";

        private readonly string _quandlApiKey;
        private readonly string _quandlStockValueUrlFormat;
        private QuandlCodesProvider _quandlCodesProvider;

        /// <summary>
        /// Creates a QuandlStockInfoService.
        /// </summary>
        /// <param name="configuration">Configuration with Quandl API parameters.</param>
        /// <param name="quandlCodesProvider">Underlying provider with list of available stock databases.</param>
        public QuandlStockInfoService(IConfiguration configuration, QuandlCodesProvider quandlCodesProvider)
        {
            _quandlApiKey = configuration["QuandlApiKey"];
            _quandlStockValueUrlFormat = configuration["QuandlStockValueUrlFormat"];
            _quandlCodesProvider = quandlCodesProvider;
        }

        /// <summary>
        /// Gets the current value per share of the specified stock.
        /// </summary>
        /// <param name="stock">Stock for which the current value per share should be returned.</param>
        /// <returns>A task that will complete with the current value per share.</returns>
        public async Task<decimal> GetCurrentShareValue(Stock stock)
        {
            var code = _quandlCodesProvider.Values.Single(x => x.Isin == stock.Isin).DatabaseCode;
            var url = string.Format(_quandlStockValueUrlFormat, code, _quandlApiKey);

            using (HttpClient client = new HttpClient())
            using (HttpResponseMessage res = await client.GetAsync(url))
            using (HttpContent content = res.Content)
            {
                string data = await content.ReadAsStringAsync();
                return GetValueFromXmlResponse(data);
            }
        }

        /// <summary>
        /// Extracts the closing value information from the XML-Response from Quandl.
        /// </summary>
        /// <param name="xmlResponse">String with the XML-Response from Quandl.</param>
        /// <returns>The closing value.</returns>
        public static decimal GetValueFromXmlResponse(string xmlResponse)
        {
            var xml = XDocument.Parse(xmlResponse);

            // Get Index of "Last" column.
            var columnIndex = Array.IndexOf(
                xml.Descendants("column-names")
                    .Descendants()
                    .InDocumentOrder()
                    .Select(x => x.Value).ToArray(), LastColumnName);

            // Get Value in "Last" column from first result row.
            // Response should only contain one row, as only the most current value is queried.
            var last = xml.Descendants("data")
                .Descendants().First()
                .Descendants()
                .InDocumentOrder().ElementAt(columnIndex)
                .Value;

            return decimal.Parse(last, CultureInfo.InvariantCulture);
        }
    }
}