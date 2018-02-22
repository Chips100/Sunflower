using System.Threading.Tasks;

namespace Sunflower.Finance.Contracts
{
    /// <summary>
    /// Provides access to real time information about individual stocks.
    /// </summary>
    public interface IStockInfoService
    {
        /// <summary>
        /// Gets the current value per share of the specified stock.
        /// </summary>
        /// <param name="isin">International Securities Identification Number of the stock.</param>
        /// <returns>A task that will complete with the current value per share.</returns>
        Task<decimal> GetCurrentShareValue(string isin);
    }
}