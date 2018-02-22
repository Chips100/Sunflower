using System;

namespace Sunflower.Entities
{
    /// <summary>
    /// Represents a transaction of money that has no context.
    /// </summary>
    /// <remarks>
    /// Used to change the balance of an account without any
    /// associated action; for example to provide the initial balance.
    /// </remarks>
    public sealed class ContextFreeTransaction : EntityBase
    {
        /// <summary>
        /// Id of the account that is affected by the transaction.
        /// </summary>
        public int AccountId { get; set; }

        /// <summary>
        /// Amount that has been added to the account (can be negative).
        /// </summary>
        public decimal Amount { get; set; }

        /// <summary>
        /// Comment explaining the reason or details of the transaction.
        /// </summary>
        public string Comment { get; set; }

        /// <summary>
        /// Timestamp of the transaction.
        /// </summary>
        public DateTime TransactionTimestamp { get; set; }
    }
}
