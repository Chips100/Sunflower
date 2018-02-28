using Sunflower.Data.Contracts;
using Sunflower.Entities;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace Sunflower.Business.Extensions
{
    /// <summary>
    /// Extension methods for common types of queries.
    /// </summary>
    public static class EntityQuerySourceExtensions
    {
        /// <summary>
        /// Queries an entity by its identifier.
        /// </summary>
        /// <typeparam name="TEntity">Type of the entity to retreive.</typeparam>
        /// <param name="context">Query Source to use for retreiving the entity.</param>
        /// <param name="id">Identifier of the entity to retreive.</param>
        /// <returns>The entity of the specified type with the specified identifier.</returns>
        public static Task<TEntity> GetById<TEntity>(this IEntityQuerySource context, int id)
            where TEntity : EntityBase
        {
            if (context == null) throw new ArgumentNullException(nameof(context));

            return context.FirstOrDefault(q => q.Get<TEntity>().Where(x => x.Id == id));
        }
    }
}