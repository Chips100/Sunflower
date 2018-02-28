using Sunflower.Data.Contracts;
using Sunflower.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Sunflower.Business.Tests.Mocks
{
    /// <summary>
    /// Mocks a persistent storage by providing methods
    /// for manipulating and querying the one underlying data source.
    /// </summary>
    public class EntityRepositoryMock : IEntityRepositoryFactory, IEntityQuerySource, IEntityQueryCollection, IEntityRepository
    {
        /// <summary>
        /// Dictionary holding collections for each type of stored entities.
        /// </summary>
        private readonly IDictionary<Type, object> _entities = new Dictionary<Type, object>();

        /// <summary>
        /// Gets the collection of entities of the specified type in the underlying data source.
        /// If it does not yet exist, a new list will be created.
        /// </summary>
        /// <typeparam name="T">Type of entities.</typeparam>
        /// <returns>The collection of entities of the specified type.</returns>
        private ICollection<T> GetOrCreateCollectionForType<T>()
        {
            object collection;
            if (!_entities.TryGetValue(typeof(T), out collection))
            {
                collection = new List<T>();
                _entities.Add(typeof(T), collection);
            }

            return (ICollection<T>)collection;
        }

        /// <summary>
        /// Gets the collection of entities of the specified type in the underlying data source.
        /// </summary>
        /// <typeparam name="T">Type of the entities.</typeparam>
        /// <returns>The collection of entities of the specified type.</returns>
        public ICollection<T> GetEntities<T>()
        {
            return GetOrCreateCollectionForType<T>();
        }

        #region Implementation of Repository interfaces to make changes.
        public async Task Use(Func<IEntityRepository, Task> asyncAction)
        {
            await asyncAction(this);
        }

        public Task Use(Action<IEntityRepository> action)
        {
            action(this);
            return Task.FromResult(0);
        }

        public TEntity Add<TEntity>(TEntity entity) where TEntity : EntityBase
        {
            GetOrCreateCollectionForType<TEntity>().Add(entity);
            return entity;
        }

        public Task Change<TEntity>(int id, Action<TEntity> changes) where TEntity : EntityBase
        {
            var entity = GetOrCreateCollectionForType<TEntity>().Single(x => x.Id == id);
            changes(entity);
            return Task.FromResult(0);
        }
        public void Remove<TEntity>(TEntity entity) where TEntity : EntityBase
        {
            GetOrCreateCollectionForType<TEntity>().Remove(entity);
        }
        #endregion

        #region Implementation of Query Source to make queries.
        public Task<TResult> FirstOrDefault<TResult>(Func<IEntityQueryCollection, IQueryable<TResult>> query)
        {
            return Task.FromResult(query(this).FirstOrDefault());
        }

        public IQueryable<T> Get<T>() where T : EntityBase
        {
            return GetOrCreateCollectionForType<T>().AsQueryable();
        }

        public Task<IEnumerable<TResult>> Query<TResult>(Func<IEntityQueryCollection, IQueryable<TResult>> query)
        {
            return Task.FromResult(query(this).ToList().AsEnumerable());
        }
        #endregion
    }
}