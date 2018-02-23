using Sunflower.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Sunflower.Finance.Contracts
{
    /// <summary>
    /// Runs queries against a list of currently available stocks.
    /// </summary>
    public interface IStockQueryService
    {
        /// <summary>
        /// Returns a complete list of all currently available stocks.
        /// </summary>
        /// <returns>A task that will complete with the list of stocks.</returns>
        Task<IEnumerable<Stock>> QueryAll();
    }
}