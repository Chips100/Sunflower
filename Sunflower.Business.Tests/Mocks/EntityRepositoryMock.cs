using Sunflower.Data.Contracts;
using Sunflower.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Sunflower.Business.Tests.Mocks
{
    /// <summary>
    /// Mocks an EntityRepository storing the entities in memory.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class EntityRepositoryMock<T> : IEntityRepository<T>
        where T : EntityBase
    {
        /// <summary>
        /// Creates an EntityRepositoryMock.
        /// </summary>
        public EntityRepositoryMock()
        { }

        /// <summary>
        /// Creates an EntityRepositoryMock containing the specified entity.
        /// </summary>
        /// <param name="entity">Entity that should be inserted.</param>
        public EntityRepositoryMock(T entity)
            : this(new[] { entity })
        { }

        /// <summary>
        /// Creates an EntityRepositoryMock containing the specified entities.
        /// </summary>
        /// <param name="entities">Sequence of entities that should be inserted.</param>
        public EntityRepositoryMock(IEnumerable<T> entities)
        {
            foreach (var entity in entities)
            {
                Entities[entity.Id] = entity;
            }
        }

        /// <summary>
        /// Holds the next id that will be used for inserting an entity.
        /// </summary>
        public int IdSeedPosition { get; set; }

        /// <summary>
        /// Entities contained in this EntityRepositoryMockup.
        /// The id of each entity serves as its key.
        /// </summary>
        public IDictionary<int, T> Entities { get; } = new Dictionary<int, T>();

        /// <summary>
        /// Applies changes to the entity with the specified identifier.
        /// </summary>
        /// <param name="id">Identifier of the entity to apply the changes to.</param>
        /// <param name="changes">Action that applies changes to the entity.</param>
        /// <returns>A Task that will complete when the entity has been changed and stored.</returns>
        public Task Change(int id, Action<T> changes)
        {
            var entity = GetById(id).Result;
            changes(entity);
            return Task.FromResult(0);
        }

        /// <summary>
        /// Stores a new entity in this Mockup.
        /// </summary>
        /// <param name="entity">Entity to store.</param>
        /// <returns>A Task that will complete when the entity has been stored.</returns>
        /// <remarks>Generates and assigns a new id to the entity.</remarks>
        public Task Create(T entity)
        {
            entity.Id = IdSeedPosition++;
            Entities.Add(entity.Id, entity);
            return Task.FromResult(0);
        }

        /// <summary>
        /// Deletes the entity with the specified identifier.
        /// </summary>
        /// <param name="id">Identifier of the entity to delete.</param>
        /// <returns>A Task that will complete when the entity has been deleted.</returns>
        public Task Delete(int id)
        {
            Entities.Remove(id);
            return Task.FromResult(0);
        }

        /// <summary>
        /// Gets an entity by its identifier.
        /// </summary>
        /// <param name="id">Identifier of the entity to get.</param>
        /// <returns>A Task that will complete with the entity; or null if it does not exist.</returns>
        public Task<T> GetById(int id)
        {
            T result;
            return Task.FromResult(Entities.TryGetValue(id, out result) ? result : default(T));
        }

        /// <summary>
        /// Queries items with the specified query from the full set of entities.
        /// </summary>
        /// <typeparam name="TTarget">Type of the queried items.</typeparam>
        /// <param name="query">Query to apply onto the full set of entities.</param>
        /// <returns>A Task that will complete with a sequence of the queried items.</returns>
        public Task<IEnumerable<TTarget>> Query<TTarget>(Func<IQueryable<T>, IQueryable<TTarget>> query)
        {
            return Task.FromResult(query(Entities.Values.AsQueryable()).ToList().AsEnumerable());
        }

        /// <summary>
        /// Gets the first item of a query applied onto the full set of entities.
        /// </summary>
        /// <typeparam name="TTarget">Type of the queried item.</typeparam>
        /// <param name="query">Query to apply onto the full set of entities and select the first item from.</param>
        /// <returns>A Task that will complete with the first queried item; or null if the query yielded no items.</returns>
        public Task<TTarget> QueryFirstOrDefault<TTarget>(Func<IQueryable<T>, IQueryable<TTarget>> query)
        {
            return Task.FromResult(query(Entities.Values.AsQueryable()).FirstOrDefault());
        }
    }
}