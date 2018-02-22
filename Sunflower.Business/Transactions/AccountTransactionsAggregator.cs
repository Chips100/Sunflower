using Sunflower.Data.Contracts;
using Sunflower.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Sunflower.Business.Transactions
{
    /// <summary>
    /// Aggregates transactions of individual accounts.
    /// </summary>
    public sealed class AccountTransactionsAggregator
    {
        private readonly IEntityRepository<ContextFreeTransaction> _contextFreeTransactionRepository;
        private readonly IEntityRepository<StockTransaction> _stockTransactionRepository;
        private readonly TransactionsMapper _mapper = new TransactionsMapper();

        /// <summary>
        /// Creates an AccountTransactionsAggregator.
        /// </summary>
        /// <param name="contextFreeTransactionRepository">Repository to query ContextFreeTransactions of an account.</param>
        /// <param name="stockTransactionRepository">Repository to query StockTransactions of an account.</param>
        public AccountTransactionsAggregator(
            IEntityRepository<ContextFreeTransaction> contextFreeTransactionRepository,
            IEntityRepository<StockTransaction> stockTransactionRepository)
        {
            _contextFreeTransactionRepository = contextFreeTransactionRepository;
            _stockTransactionRepository = stockTransactionRepository;
        }

        /// <summary>
        /// Aggregates all transactions of a single account.
        /// </summary>
        /// <param name="accountId">Id of the account.</param>
        /// <returns>A task that completes with the result of the aggregation.</returns>
        public async Task<AggregationResult> AggregateTransactionsOfAccount(int accountId)
        {
            // Query all transactions of the account.
            var contextFreeTransactions = await _contextFreeTransactionRepository.Query(q => q.Where(x => x.AccountId == accountId));
            var stockTransactions = await _stockTransactionRepository.Query(q => q.Where(x => x.AccountId == accountId));

            // Map those transactions to AggregationItems.
            var aggregationItems = contextFreeTransactions.Select(_mapper.Map)
                .Concat(stockTransactions.Select(_mapper.Map));

            // Aggregate the items to the final result.
            return AggregateItems(aggregationItems);
        }

        /// <summary>
        /// Aggregates a sequence of items to a final result.
        /// </summary>
        /// <param name="items">Items that should be aggregated.</param>
        /// <returns>The result from the aggregation.</returns>
        private AggregationResult AggregateItems(IEnumerable<IAggregationItem> items)
        {
            if (items == null) throw new ArgumentNullException(nameof(items));

            // Start with an empty accumulator.
            var accumulator = AggregationAccumulator.Empty;

            // Apply changes of each item.
            foreach(var item in items)
            {
                item.Apply(accumulator);
            }

            // Transform the final state into the result object.
            return accumulator.Finalize();
        }
    }
}