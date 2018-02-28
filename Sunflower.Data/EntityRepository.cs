using Microsoft.EntityFrameworkCore;
using Sunflower.Data.Contracts;
using Sunflower.Entities;
using System;
using System.Threading.Tasks;

namespace Sunflower.Data
{
    /// <summary>
    /// Defines operations to make changes to the persistent storage of entities.
    /// </summary>
    public class EntityRepository : IEntityRepository
    {
        private readonly SunflowerDbContext _db;

        /// <summary>
        /// Creates an EntityRepository that uses an Entity Framework context.
        /// </summary>
        /// <param name="db">Entity Framework context used to make changes.</param>
        public EntityRepository(SunflowerDbContext db)
        {
            _db = db;
        }

        /// <summary>
        /// Stores a new entity in the persistent storage.
        /// </summary>
        /// <param name="entity">Entity to store.</param>
        /// <typeparam name="TEntity">Type of the entity to store.</typeparam>
        /// <returns>The new entity that was added to the persistent storage.</returns>
        public TEntity Add<TEntity>(TEntity entity) where TEntity : EntityBase
        {
            _db.Add(entity);
            return entity;
        }

        /// <summary>
        /// Makes changes to an existing entity in the persistent storage.
        /// </summary>
        /// <typeparam name="TEntity">Type of the stored entity.</typeparam>
        /// <param name="id">Identifier of the entity that should be changed.</param>
        /// <param name="changes">Changes to apply to the entity.</param>
        /// <returns>A task that will complete when the changes have been applied.</returns>
        public async Task Change<TEntity>(int id, Action<TEntity> changes) where TEntity : EntityBase
        {
            var entity = await _db.Set<TEntity>().SingleAsync(x => x.Id == id);
            changes(entity);
        }

        /// <summary>
        /// Deletes an entity from the persistent storage.
        /// </summary>
        /// <param name="id">Identifier of the entity to delete.</param>
        /// <typeparam name="TEntity">Type of the entity to delete.</typeparam>
        public void Remove<TEntity>(TEntity entity) where TEntity : EntityBase
        {
            _db.Remove(entity);
        }
    }
}