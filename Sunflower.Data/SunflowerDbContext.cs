using Microsoft.EntityFrameworkCore;
using Sunflower.Entities;

namespace Sunflower.Data
{
    /// <summary>
    /// Defines an EntityFramework context used for accessing entities.
    /// </summary>
    public sealed class SunflowerDbContext : DbContext
    {
        /// <summary>
        /// Set of all Account entities in the persistent storage.
        /// </summary>
        public DbSet<Account> Accounts { get; set; }

        /// <summary>
        /// Defines operations to configure EntityFramework.
        /// </summary>
        /// <param name="optionsBuilder">Builder used to configure EntityFramework.</param>
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlite("Filename=./Sunflower.db");
            base.OnConfiguring(optionsBuilder);
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Account>().HasIndex(x => x.EmailAddress).IsUnique();
            base.OnModelCreating(modelBuilder);
        }
    }
}