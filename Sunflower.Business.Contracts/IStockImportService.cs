using System.Threading.Tasks;

namespace Sunflower.Business.Contracts
{
    /// <summary>
    /// Allows importing the list of currently available stocks
    /// into the Sunflower storage for usage in the application.
    /// </summary>
    public interface IStockImportService
    {
        /// <summary>
        /// Imports the list of currently available stocks.
        /// </summary>
        /// <returns>A task that will complete when the import has completed.</returns>
        Task ImportStocks();
    }
}