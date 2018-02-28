using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Sunflower.Data.Contracts
{
    /// <summary>
    /// Allows making queries to the persistent storage.
    /// </summary>
    public interface IEntityQuerySource
    {
        /// <summary>
        /// Queries the persistent storage to retreive a list of results.
        /// </summary>
        /// <typeparam name="TResult">Type of the results of the query.</typeparam>
        /// <param name="query">Query to build from the entity sources.</param>
        /// <returns>A task that will complete with the results of the query.</returns>
        Task<IEnumerable<TResult>> Query<TResult>(Func<IEntityQueryCollection, IQueryable<TResult>> query);

        /// <summary>
        /// Queries the persistent storage to retreive the first matching result.
        /// </summary>
        /// <typeparam name="TResult">Type of the results of the query.</typeparam>
        /// <param name="query">Query to build from the entity sources.</param>
        /// <returns>A task that will complete with the first result of the query.</returns>
        Task<TResult> FirstOrDefault<TResult>(Func<IEntityQueryCollection, IQueryable<TResult>> query);
    }
}