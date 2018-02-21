using Sunflower.Data.Contracts;
using System.Threading.Tasks;

namespace Sunflower.Data
{
    /// <summary>
    /// Defines operations to create the persistent storage in which entities will be stored.
    /// </summary>
    public class SunflowerStorageCreator : IStorageCreator
    {
        /// <summary>
        /// Creates the persistent storage if it does not exist yet.
        /// </summary>
        /// <returns>A Task that will complete when the storage has been created.</returns>
        public async Task EnsureCreated()
        {
            using (var context = new SunflowerDbContext())
            {
                await context.Database.EnsureCreatedAsync();
            }
        }
    }
}