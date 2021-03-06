﻿using Sunflower.Entities;
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
        /// <param name="stock">Stock for which the current value per share should be returned.</param>
        /// <returns>A task that will complete with the current value per share.</returns>
        Task<decimal> GetCurrentShareValue(Stock stock);
    }
}