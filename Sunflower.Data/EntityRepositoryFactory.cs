using Sunflower.Data.Contracts;
using System;
using System.Threading.Tasks;

namespace Sunflower.Data
{
    /// <summary>
    /// Factory for repositories to make changes to the persistent storage.
    /// </summary>
    public sealed class EntityRepositoryFactory : IEntityRepositoryFactory
    {
        /// <summary>
        /// Provides a repository to make changes to the persistent storage.
        /// </summary>
        /// <param name="action">Action to perform changes with the repository.</param>
        /// <returns>A task that will complete when the changes have been applied.</returns>
        public async Task Use(Action<IEntityRepository> action)
        {
            using (var context = CreateContext())
            {
                action(new EntityRepository(context));
                await context.SaveChangesAsync();
            }
        }

        /// <summary>
        /// Provides a repository to make changes to the persistent storage.
        /// </summary>
        /// <param name="action">Asynchronous action to perform changes with the repository.</param>
        /// <returns>A task that will complete when the changes have been applied.</returns>
        public async Task Use(Func<IEntityRepository, Task> asyncAction)
        {
            using (var context = CreateContext())
            {
                await asyncAction(new EntityRepository(context));
                await context.SaveChangesAsync();
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