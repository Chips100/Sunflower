using System;

namespace Sunflower.Entities
{
    /// <summary>
    /// Represents a transaction that resulted from buying
    /// or selling shares of a stock.
    /// </summary>
    public sealed class StockTransaction : EntityBase
    {
        /// <summary>
        /// Id of the account that is affected by the transaction.
        /// </summary>
        public int AccountId { get; set; }

        /// <summary>
        /// Id of the stock that is the target of the transaction.
        /// </summary>
        public int StockId { get; set; }

        /// <summary>
        /// Number of shares that were bought. This value
        /// can be negative to indicate selling of shares.
        /// </summary>
        public decimal SharesCount { get; set; }

        /// <summary>
        /// The value of a single share of the stock at
        /// the moment of this transaction. Is always positive.
        /// </summary>
        public decimal ShareValue { get; set; }

        /// <summary>
        /// Timestamp of the transaction.
        /// </summary>
        public DateTime TransactionTimestamp { get; set; }
    }
}