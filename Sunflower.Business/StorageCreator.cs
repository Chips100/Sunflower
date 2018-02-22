using Sunflower.Business.Contracts;
using Sunflower.Business.Setup;
using System.Threading.Tasks;

namespace Sunflower.Business
{
    /// <summary>
    /// Defines operations to create the storage that is needed for managing entities.
    /// </summary>
    public class StorageCreator : IStorageCreator
    {
        private readonly Data.Contracts.IStorageCreator _dataStorageCreator;
        private readonly StockListInitializer _stockListInitializer;

        /// <summary>
        /// Creates a StorageCreator.
        /// </summary>
        /// <param name="dataStorageCreator">StorageCreator used to create a persistent storage.</param>
        public StorageCreator(Data.Contracts.IStorageCreator dataStorageCreator, StockListInitializer stockListInitializer)
        {
            _dataStorageCreator = dataStorageCreator;
            _stockListInitializer = stockListInitializer;
        }

        /// <summary>
        /// Creates the storage if it does not exist yet.
        /// </summary>
        /// <returns>A Task that will complete when the storage has been created.</returns>
        public async Task EnsureCreated()
        {
            await _dataStorageCreator.EnsureCreated();
            await _stockListInitializer.InitializeStockList();
        }
    }
}