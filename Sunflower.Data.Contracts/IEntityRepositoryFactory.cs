using System;
using System.Threading.Tasks;

namespace Sunflower.Data.Contracts
{
    /// <summary>
    /// Factory for repositories to make changes to the persistent storage.
    /// </summary>
    public interface IEntityRepositoryFactory
    {
        /// <summary>
        /// Provides a repository to make changes to the persistent storage.
        /// </summary>
        /// <param name="action">Action to perform changes with the repository.</param>
        /// <returns>A task that will complete when the changes have been applied.</returns>
        Task Use(Action<IEntityRepository> action);

        /// <summary>
        /// Provides a repository to make changes to the persistent storage.
        /// </summary>
        /// <param name="action">Asynchronous action to perform changes with the repository.</param>
        /// <returns>A task that will complete when the changes have been applied.</returns>
        Task Use(Func<IEntityRepository, Task> asyncAction);
    }
}