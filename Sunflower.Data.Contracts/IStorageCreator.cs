using System.Threading.Tasks;

namespace Sunflower.Data.Contracts
{
    /// <summary>
    /// Defines operations to create the persistent storage in which entities will be stored.
    /// </summary>
    public interface IStorageCreator
    {
        /// <summary>
        /// Creates the persistent storage if it does not exist yet.
        /// </summary>
        /// <returns>A Task that will complete when the storage has been created.</returns>
        Task EnsureCreated();
    }
}