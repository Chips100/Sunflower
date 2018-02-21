using Microsoft.EntityFrameworkCore;
using Sunflower.Entities;
using System.Threading.Tasks;

namespace Sunflower.Data
{
    /// <summary>
    /// Defines extension methods on the <see cref="SunflowerDbContext"/>. 
    /// </summary>
    public static class SunflowerDbContextExtensions
    {
        /// <summary>
        /// Gets the entity of a specific type with the specified identifier.
        /// </summary>
        /// <typeparam name="T">Type of the entity to get.</typeparam>
        /// <param name="context">Context used for getting the entity.</param>
        /// <param name="id">Identifier of the entity to get.</param>
        /// <returns>A Task that will complete with the entity.</returns>
        public static async Task<T> GetEntityById<T>(this SunflowerDbContext context, int id)
            where T : EntityBase
        {
            return await context.Set<T>().SingleAsync(x => x.Id == id);
        }
    }
}