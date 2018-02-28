using Sunflower.Business.Contracts;
using Sunflower.Business.Extensions;
using Sunflower.Business.Transactions;
using Sunflower.Data.Contracts;
using Sunflower.Entities;
using Sunflower.Finance.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Sunflower.Business
{
    public sealed class StockService : IStockService
    {
        private readonly IEntityQuerySource _entityQuerySource;
        private readonly IEntityRepositoryFactory _entityRepositoryFactory;
        private AccountTransactionsAggregator _accountTransactionsAggregator;
        private IStockInfoService _stockInfoService;

        /// <summary>
        /// Creates a StockService.
        /// </summary>
        /// <param name="entityQuerySource">EntityQuerySource to run queries against the persistent storage.</param>
        /// <param name="entityRepositoryFactory">EntityRepositoryFactory to make changes to the persistent storage.</param>
        /// <param name="accountTransactionsAggregator"></param>
        /// <param name="stockInfoService"></param>
        public StockService(IEntityQuerySource entityQuerySource, IEntityRepositoryFactory entityRepositoryFactory, AccountTransactionsAggregator accountTransactionsAggregator, IStockInfoService stockInfoService)
        {
            _entityQuerySource = entityQuerySource;
            _entityRepositoryFactory = entityRepositoryFactory;
            _stockInfoService = stockInfoService;
            _accountTransactionsAggregator = accountTransactionsAggregator;
        }

        /// <summary>
        /// Transfers shares of a stock to the specified account.
        /// </summary>
        /// <param name="accountId">Id of the account that "buys" the shares.</param>
        /// <param name="stockId">Id of the stock of which shares are "bought".</param>
        /// <param name="sharesCount">Number of shares that should be "bought".</param>
        /// <param name="maxShareValue">Maximum value for an individual share; if the current value exceeds this value, an exception will be thrown.</param>
        /// <returns>A task that completes when the operation has been completed.</returns>
        public async Task BuyShares(int accountId, int stockId, int sharesCount, decimal? maxShareValue)
        {
            var accountStatus = await _accountTransactionsAggregator.AggregateTransactionsOfAccount(accountId);
            var stock = await _entityQuerySource.GetById<Stock>(stockId);
            var shareValue = await _stockInfoService.GetCurrentShareValue(stock);

            if (shareValue > maxShareValue)
            {
                throw new Exception("Zu teuer");
            }

            var total = shareValue * sharesCount;
            if (total > accountStatus.Balance)
            {
                throw new Exception("Zu wenig guthaben");
            }

            await _entityRepositoryFactory.Use(repository =>
            {
                repository.Add(new StockTransaction
                {
                    AccountId = accountId,
                    SharesCount = sharesCount,
                    ShareValue = shareValue,
                    TransactionTimestamp = DateTime.UtcNow,
                    StockId = stockId
                });
            });
        }

        /// <summary>
        /// Searches for stocks matching the specified search term.
        /// </summary>
        /// <param name="searchTerm">Term to use for matching stocks.</param>
        /// <returns>A task that will complete with the matched stocks.</returns>
        public async Task<IEnumerable<Stock>> SearchStocks(string searchTerm)
        {
            return await _entityQuerySource.Query(q =>
                q.Get<Stock>().Where(s => s.Name.Contains(searchTerm)));
        }

        /// <summary>
        /// Removes shares of a stock from the specified account.
        /// </summary>
        /// <param name="accountId">Id of the account that "sells" the shares.</param>
        /// <param name="stockId">Id of the stock of which shares are "sold".</param>
        /// <param name="sharesCount">Number of shares that should be "sold".</param>
        /// <param name="maxShareValue">Minimum selling value for an individual share; if the current value is lower than this value, an exception will be thrown.</param>
        /// <returns>A task that completes when the operation has been completed.</returns>
        public Task SellShares(int accountId, int stockId, int sharesCount, decimal? minShareValue)
        {
            throw new NotImplementedException();
        }
    }
}