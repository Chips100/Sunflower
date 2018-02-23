using Sunflower.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Sunflower.Data.Contracts
{
    /// <summary>
    /// Defines operations to access entities in the persistent storage.
    /// </summary>
    /// <typeparam name="TEntity">Type of the entities that can be accessed with this repository.</typeparam>
    /// <remarks>All operations are defined as asynchronous operations.</remarks>
    public interface IEntityRepository<TEntity>
        where TEntity : EntityBase
    {
        /// <summary>
        /// Applies changes to the entity with the specified identifier.
        /// </summary>
        /// <param name="id">Identifier of the entity to apply the changes to.</param>
        /// <param name="changes">Action that applies changes to the entity.</param>
        /// <returns>A Task that will complete when the entity has been changed and stored.</returns>
        Task Change(int id, Action<TEntity> changes);

        /// <summary>
        /// Stores a new entity in the persistent storage.
        /// </summary>
        /// <param name="entity">Entity to store.</param>
        /// <returns>A Task that will complete with the stored entity.</returns>
        /// <remarks>Generates and assigns a new id to the entity.</remarks>
        Task<TEntity> Create(TEntity entity);

        /// <summary>
        /// Deletes the entity with the specified identifier.
        /// </summary>
        /// <param name="id">Identifier of the entity to delete.</param>
        /// <returns>A Task that will complete when the entity has been deleted.</returns>
        Task Delete(int id);

        /// <summary>
        /// Gets an entity by its identifier.
        /// </summary>
        /// <param name="id">Identifier of the entity to get.</param>
        /// <returns>A Task that will complete with the entity; or null if it does not exist.</returns>
        Task<TEntity> GetById(int id);

        /// <summary>
        /// Queries items with the specified query from the full set of entities.
        /// </summary>
        /// <typeparam name="TTarget">Type of the queried items.</typeparam>
        /// <param name="query">Query to apply onto the full set of entities.</param>
        /// <returns>A Task that will complete with a sequence of the queried items.</returns>
        Task<IEnumerable<TTarget>> Query<TTarget>(Func<IQueryable<TEntity>, IQueryable<TTarget>> query);

        /// <summary>
        /// Gets the first item of a query applied onto the full set of entities.
        /// </summary>
        /// <typeparam name="TTarget">Type of the queried item.</typeparam>
        /// <param name="query">Query to apply onto the full set of entities and select the first item from.</param>
        /// <returns>A Task that will complete with the first queried item; or null if the query yielded no items.</returns>
        Task<TTarget> QueryFirstOrDefault<TTarget>(Func<IQueryable<TEntity>, IQueryable<TTarget>> query);
    }
}