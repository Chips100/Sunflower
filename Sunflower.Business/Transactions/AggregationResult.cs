using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace Sunflower.Business.Transactions
{
    /// <summary>
    /// Represents the result of aggregating all transactions of an account
    /// to determine the current state.
    /// </summary>
    public sealed class AggregationResult
    {
        /// <summary>
        /// Creates an AggregationResult.
        /// </summary>
        /// <param name="balance">Current balance of the account after aggregating all transactions.</param>
        /// <param name="stockShares">Current counts of shares owned by the account for stocks.</param>
        public AggregationResult(decimal balance, IDictionary<int, decimal> stockShares)
        {
            if (stockShares == null) throw new ArgumentNullException(nameof(stockShares));

            Balance = balance;
            StockShares = new ReadOnlyDictionary<int, decimal>(stockShares);
        }

        /// <summary>
        /// Gets the current balance of the account after aggregating all transactions.
        /// </summary>
        public decimal Balance { get; }

        /// <summary>
        /// Gets the current counts of shares owned by the account for stocks.
        /// Key = Id of the stock; Value = count of shares owned.
        /// </summary>
        public IReadOnlyDictionary<int, decimal> StockShares { get; }
    }
}
