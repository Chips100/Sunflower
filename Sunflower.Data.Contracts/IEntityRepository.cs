using Sunflower.Entities;
using System;
using System.Threading.Tasks;

namespace Sunflower.Data.Contracts
{
    /// <summary>
    /// Defines operations to make changes to the persistent storage of entities.
    /// </summary>
    public interface IEntityRepository
    {
        /// <summary>
        /// Stores a new entity in the persistent storage.
        /// </summary>
        /// <param name="entity">Entity to store.</param>
        /// <typeparam name="TEntity">Type of the entity to store.</typeparam>
        /// <returns>The new entity that was added to the persistent storage.</returns>
        TEntity Add<TEntity>(TEntity entity)
            where TEntity : EntityBase;

        /// <summary>
        /// Makes changes to an existing entity in the persistent storage.
        /// </summary>
        /// <typeparam name="TEntity">Type of the stored entity.</typeparam>
        /// <param name="id">Identifier of the entity that should be changed.</param>
        /// <param name="changes">Changes to apply to the entity.</param>
        /// <returns>A task that will complete when the changes have been applied.</returns>
        Task Change<TEntity>(int id, Action<TEntity> changes)
            where TEntity : EntityBase;

        /// <summary>
        /// Deletes an entity from the persistent storage.
        /// </summary>
        /// <param name="id">Identifier of the entity to delete.</param>
        /// <typeparam name="TEntity">Type of the entity to delete.</typeparam>
        void Remove<TEntity>(TEntity entity)
            where TEntity : EntityBase;
    }
}