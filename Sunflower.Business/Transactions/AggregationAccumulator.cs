using System.Collections.Generic;

namespace Sunflower.Business.Transactions
{
    /// <summary>
    /// Represents an accumulator used for aggregating items.
    /// Aggregation always starts with an empty accumulator; each item
    /// applies its changes and the final result is converted into an <see cref="AggregationResult"/>.
    /// </summary>
    public sealed class AggregationAccumulator
    {
        private decimal _balance = 0;
        private IDictionary<int, decimal> _stockShares = new Dictionary<int, decimal>();

        /// <summary>
        /// Private constructor to force starting with an empty accumulator.
        /// </summary>
        private AggregationAccumulator() { }

        /// <summary>
        /// Applies a change to the current balance.
        /// </summary>
        /// <param name="change">Change that should be applied; can be negative.</param>
        public void ChangeBalance(decimal change)
        {
            _balance += change;
        }

        /// <summary>
        /// Applies a change to the shares hold by the accumulator.
        /// </summary>
        /// <param name="stockId">Id of the stock of which shares are added / removed.</param>
        /// <param name="change">Change to the number of shares owned; can be negative.</param>
        public void ChangeShares(int stockId, decimal change)
        {
            // Initialize counter with 0, if not yet present.
            if (!_stockShares.ContainsKey(stockId))
            {
                _stockShares[stockId] = 0;
            }

            _stockShares[stockId] += change;
        }

        /// <summary>
        /// Transforms the current state into an AggregationResult.
        /// </summary>
        /// <returns>An AggregationResult with the current state of the accumulator.</returns>
        public AggregationResult Finalize() => new AggregationResult(_balance, _stockShares);

        /// <summary>
        /// Creates an empty accumulator to start aggregating.
        /// </summary>
        public static AggregationAccumulator Empty => new AggregationAccumulator();
    }
}