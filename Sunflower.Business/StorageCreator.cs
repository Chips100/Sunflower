using Sunflower.Business.Contracts;
using System.Threading.Tasks;

namespace Sunflower.Business
{
    /// <summary>
    /// Defines operations to create the storage that is needed for managing entities.
    /// </summary>
    public class StorageCreator : IStorageCreator
    {
        private readonly Data.Contracts.IStorageCreator _dataStorageCreator;

        /// <summary>
        /// Creates a StorageCreator.
        /// </summary>
        /// <param name="dataStorageCreator">StorageCreator used to create a persistent storage.</param>
        public StorageCreator(Data.Contracts.IStorageCreator dataStorageCreator)
        {
            _dataStorageCreator = dataStorageCreator;
        }

        /// <summary>
        /// Creates the storage if it does not exist yet.
        /// </summary>
        /// <returns>A Task that will complete when the storage has been created.</returns>
        public async Task EnsureCreated()
        {
            await _dataStorageCreator.EnsureCreated();
        }
    }
}