using System.Threading.Tasks;

namespace Sunflower.Business.Contracts
{
    /// <summary>
    /// Defines operations to create the storage that is needed for managing entities.
    /// </summary>
    public interface IStorageCreator
    {
        /// <summary>
        /// Creates the storage if it does not exist yet.
        /// </summary>
        /// <returns>A Task that will complete when the storage has been created.</returns>
        Task EnsureCreated();
    }
}