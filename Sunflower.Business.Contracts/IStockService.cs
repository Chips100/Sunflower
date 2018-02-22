using System.Threading.Tasks;

namespace Sunflower.Business.Contracts
{
    /// <summary>
    /// Defines operations to deal with stocks.
    /// </summary>
    public interface IStockService
    {
        /// <summary>
        /// Transfers shares of a stock to the specified account.
        /// </summary>
        /// <param name="accountId">Id of the account that "buys" the shares.</param>
        /// <param name="stockId">Id of the stock of which shares are "bought".</param>
        /// <param name="sharesCount">Number of shares that should be "bought".</param>
        /// <param name="maxShareValue">Maximum value for an individual share; if the current value exceeds this value, an exception will be thrown.</param>
        /// <returns>A task that completes when the operation has been completed.</returns>
        Task BuyShares(int accountId, int stockId, int sharesCount, decimal? maxShareValue);

        /// <summary>
        /// Removes shares of a stock from the specified account.
        /// </summary>
        /// <param name="accountId">Id of the account that "sells" the shares.</param>
        /// <param name="stockId">Id of the stock of which shares are "sold".</param>
        /// <param name="sharesCount">Number of shares that should be "sold".</param>
        /// <param name="maxShareValue">Minimum selling value for an individual share; if the current value is lower than this value, an exception will be thrown.</param>
        /// <returns>A task that completes when the operation has been completed.</returns>
        Task SellShares(int accountId, int stockId, int sharesCount, decimal? minShareValue);
    }
}
