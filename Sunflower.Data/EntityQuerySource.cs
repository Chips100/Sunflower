using Microsoft.EntityFrameworkCore;
using Sunflower.Data.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Sunflower.Data
{
    /// <summary>
    /// Allows making queries to the persistent storage.
    /// </summary>
    public sealed class EntityQuerySource : IEntityQuerySource
    {
        /// <summary>
        /// Queries the persistent storage to retreive a list of results.
        /// </summary>
        /// <typeparam name="TResult">Type of the results of the query.</typeparam>
        /// <param name="query">Query to build from the entity sources.</param>
        /// <returns>A task that will complete with the results of the query.</returns>
        public async Task<IEnumerable<TResult>> Query<TResult>(Func<IEntityQueryCollection, IQueryable<TResult>> query)
        {
            using (var context = CreateContext())
            {
                var queryable = query(new EntityQueryCollection(context));
                return await queryable.ToListAsync();
            }
        }

        /// <summary>
        /// Queries the persistent storage to retreive the first matching result.
        /// </summary>
        /// <typeparam name="TResult">Type of the results of the query.</typeparam>
        /// <param name="query">Query to build from the entity sources.</param>
        /// <returns>A task that will complete with the first result of the query.</returns>
        public async Task<TResult> FirstOrDefault<TResult>(Func<IEntityQueryCollection, IQueryable<TResult>> query)
        {
            using (var context = CreateContext())
            {
                var queryable = query(new EntityQueryCollection(context));
                return await queryable.FirstOrDefaultAsync();
            }
        }

        /// <summary>
        /// Creates an EntityFramework context to access the persistent storage.
        /// </summary>
        /// <returns>An EntityFramework context.</returns>
        private SunflowerDbContext CreateContext()
        {
            return new SunflowerDbContext();
        }
    }
}