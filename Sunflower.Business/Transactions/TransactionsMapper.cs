using Sunflower.Entities;
using System;

namespace Sunflower.Business.Transactions
{
    /// <summary>
    /// Maps entities representing transactions to corresponding items
    /// suitable for aggregation (<see cref="IAggregationItem"/>).
    /// </summary>
    public sealed class TransactionsMapper
    {
        /// <summary>
        /// Maps a ContextFreeTransaction to an IAggregationItem.
        /// </summary>
        /// <param name="transaction">Transaction that should be mapped.</param>
        /// <returns>An IAggregationItem representing the change to the balance caused by the transaction.</returns>
        public IAggregationItem Map(ContextFreeTransaction transaction)
        {
            if (transaction == null) throw new ArgumentNullException(nameof(transaction));

            return new AggregationItem(transaction.Amount, null, 0);
        }

        /// <summary>
        /// Maps a StockTransaction to an IAggregationItem.
        /// </summary>
        /// <param name="transaction">Transaction that should be mapped.</param>
        /// <returns>An IAggregationItem representing the change to the balance and shares caused by the transaction.</returns>
        public IAggregationItem Map(StockTransaction transaction)
        {
            if (transaction == null) throw new ArgumentNullException(nameof(transaction));

            // -1, because a positive change to the shares causes a negative change to the balance.
            return new AggregationItem(
                -1 * transaction.SharesCount * transaction.ShareValue,
                transaction.StockId,
                transaction.SharesCount
            );
        }

        /// <summary>
        /// Internal representation of an IAggregationItem that all
        /// transactions are mapped to.
        /// </summary>
        private class AggregationItem : IAggregationItem
        {
            private readonly decimal _balanceChange;
            private readonly int? _stockId;
            private readonly decimal _sharesChange;

            /// <summary>
            /// Creates an AggregationItem.
            /// </summary>
            /// <param name="balanceChange">Change to the balance that the AggregationItem should cause.</param>
            /// <param name="stockId">Optional Id of the stock of which shares are added or removed by the transaction.</param>
            /// <param name="sharesChange">Change to the number of shares of the corresponding stock. that the AggregationItem should cause.</param>
            public AggregationItem(decimal balanceChange, int? stockId, decimal sharesChange)
            {
                _balanceChange = balanceChange;
                _stockId = stockId;
                _sharesChange = sharesChange;
            }

            /// <summary>
            /// Applies the changes represented by this AggregationItem to the accumulator.
            /// </summary>
            /// <param name="accumulator">Accumulator on which the changes should be applied.</param>
            public void Apply(AggregationAccumulator accumulator)
            {
                // Always apply corresponding change to balance.
                accumulator.ChangeBalance(_balanceChange);

                // If shares are transferred by this item, apply those changes as well.
                if (_stockId.HasValue)
                {
                    accumulator.ChangeShares(_stockId.Value, _sharesChange);
                }
            }
        }
    }
}