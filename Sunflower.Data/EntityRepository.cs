using Microsoft.EntityFrameworkCore;
using Sunflower.Data.Contracts;
using Sunflower.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Sunflower.Data
{
    /// <summary>
    /// Defines operations to access entities in the persistent storage using Entity Framework.
    /// </summary>
    /// <typeparam name="TEntity">Type of the entities that can be accessed with this repository.</typeparam>
    /// <remarks>All operations are defined as asynchronous operations.</remarks>
    public class EntityRepository<T> : IEntityRepository<T>
        where T : EntityBase
    {
        /// <summary>
        /// Applies changes to the entity with the specified identifier.
        /// </summary>
        /// <param name="id">Identifier of the entity to apply the changes to.</param>
        /// <param name="changes">Action that applies changes to the entity.</param>
        /// <returns>A Task that will complete when the entity has been changed and stored.</returns>
        public async Task Change(int id, Action<T> changes)
        {
            using (var context = CreateContext())
            {
                var entity = await context.GetEntityById<T>(id);
                changes(entity);
                await context.SaveChangesAsync();
            }
        }

        /// <summary>
        /// Stores a new entity in the persistent storage.
        /// </summary>
        /// <param name="entity">Entity to store.</param>
        /// <returns>A Task that will complete when the entity has been stored.</returns>
        /// <remarks>Generates and assigns a new id to the entity.</remarks>
        public async Task Create(T entity)
        {
            using (var context = CreateContext())
            {
                context.Set<T>().Add(entity);
                await context.SaveChangesAsync();
            }
        }

        /// <summary>
        /// Deletes the entity with the specified identifier.
        /// </summary>
        /// <param name="id">Identifier of the entity to delete.</param>
        /// <returns>A Task that will complete when the entity has been deleted.</returns>
        public async Task Delete(int id)
        {
            using (var context = CreateContext())
            {
                var entity = await context.GetEntityById<T>(id);
                context.Set<T>().Remove(entity);
                await context.SaveChangesAsync();
            }
        }

        /// <summary>
        /// Gets an entity by its identifier.
        /// </summary>
        /// <param name="id">Identifier of the entity to get.</param>
        /// <returns>A Task that will complete with the entity; or null if it does not exist.</returns>
        public Task<T> GetById(int id)
        {
            using (var context = CreateContext())
            {
                return context.GetEntityById<T>(id);
            }
        }

        /// <summary>
        /// Queries items with the specified query from the full set of entities.
        /// </summary>
        /// <typeparam name="TTarget">Type of the queried items.</typeparam>
        /// <param name="query">Query to apply onto the full set of entities.</param>
        /// <returns>A Task that will complete with a sequence of the queried items.</returns>
        public async Task<IEnumerable<TTarget>> Query<TTarget>(Func<IQueryable<T>, IQueryable<TTarget>> query)
        {
            using (var context = CreateContext())
            {
                return await query(context.Set<T>()).ToListAsync();
            }
        }

        /// <summary>
        /// Gets the first item of a query applied onto the full set of entities.
        /// </summary>
        /// <typeparam name="TTarget">Type of the queried item.</typeparam>
        /// <param name="query">Query to apply onto the full set of entities and select the first item from.</param>
        /// <returns>A Task that will complete with the first queried item; or null if the query yielded no items.</returns>
        public async Task<TTarget> QueryFirstOrDefault<TTarget>(Func<IQueryable<T>, IQueryable<TTarget>> query)
        {
            using (var context = CreateContext())
            {
                return await query(context.Set<T>()).FirstOrDefaultAsync();
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