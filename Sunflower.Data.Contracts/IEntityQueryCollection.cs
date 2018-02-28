using Sunflower.Entities;
using System.Linq;

namespace Sunflower.Data.Contracts
{
    /// <summary>
    /// Collection of queryables for each entity stored in the persistent storage.
    /// </summary>
    public interface IEntityQueryCollection
    {
        /// <summary>
        /// Gets a queryable to access entities of the specified type.
        /// </summary>
        /// <typeparam name="T">Type of the entities to query.</typeparam>
        /// <returns>A queryable to access entities of the specified type.</returns>
        IQueryable<T> Get<T>()
            where T : EntityBase;
    }
}