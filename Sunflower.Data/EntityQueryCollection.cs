using Sunflower.Data.Contracts;
using Sunflower.Entities;
using System.Linq;

namespace Sunflower.Data
{
    /// <summary>
    /// Collection of queryables for each entity stored in the persistent storage.
    /// </summary>
    public sealed class EntityQueryCollection : IEntityQueryCollection
    {
        private readonly SunflowerDbContext _db;

        /// <summary>
        /// Creates an EntityQueryCollection that proxies an Entity Framework context.
        /// </summary>
        /// <param name="db">Entity Framework context to use for accessing the queryables.</param>
        public EntityQueryCollection(SunflowerDbContext db)
        {
            _db = db;
        }

        /// <summary>
        /// Gets a queryable to access entities of the specified type.
        /// </summary>
        /// <typeparam name="T">Type of the entities to query.</typeparam>
        /// <returns>A queryable to access entities of the specified type.</returns>
        public IQueryable<T> Get<T>() where T : EntityBase
        {
            return _db.Set<T>();
        }
    }
}