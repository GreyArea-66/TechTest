using System;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using UserManagement.Models;
using System.Threading.Tasks;

namespace UserManagement.Data
{
    /// <summary>
    /// Represents the application's data context.
    /// </summary>
    public class DataContext : DbContext, IDataContext
    {
        /// <summary>
        /// Initializes a new instance of the DataContext class.
        /// </summary>
        public DataContext() => Database.EnsureCreated();

        /// <summary>
        /// Configures the database to be used.
        /// </summary>
        protected override void OnConfiguring(DbContextOptionsBuilder options)
            => options.UseInMemoryDatabase("UserManagement.Data.DataContext");

        /// <summary>
        /// Configures the model that was discovered by convention from the entity types.
        /// </summary>
        protected override void OnModelCreating(ModelBuilder model)
            => model.Entity<User>().HasData(new[]
            {
                new User { Id = 1, Forename = "Peter", Surname = "Loew", Email = "ploew@example.com", DateOfBirth = new DateOnly(1990, 1, 1), IsActive = true },
                new User { Id = 2, Forename = "Benjamin Franklin", Surname = "Gates", Email = "bfgates@example.com", DateOfBirth = new DateOnly(1999, 1, 31), IsActive = true },
                new User { Id = 3, Forename = "Castor", Surname = "Troy", Email = "ctroy@example.com", DateOfBirth =  new DateOnly(1994, 3, 26), IsActive = false },
                new User { Id = 4, Forename = "Memphis", Surname = "Raines", Email = "mraines@example.com", DateOfBirth = new DateOnly(1991, 8, 23) ,IsActive = true },
                new User { Id = 5, Forename = "Stanley", Surname = "Goodspeed", Email = "sgodspeed@example.com", DateOfBirth = new DateOnly(2000, 1, 28), IsActive = true },
                new User { Id = 6, Forename = "H.I.", Surname = "McDunnough", Email = "himcdunnough@example.com", DateOfBirth = new DateOnly(2000, 1, 28), IsActive = true },
                new User { Id = 7, Forename = "Cameron", Surname = "Poe", Email = "cpoe@example.com", DateOfBirth = new DateOnly(2000, 1, 28), IsActive = false },
                new User { Id = 8, Forename = "Edward", Surname = "Malus", Email = "emalus@example.com", DateOfBirth = new DateOnly(2000, 1, 28), IsActive = false },
                new User { Id = 9, Forename = "Damon", Surname = "Macready", Email = "dmacready@example.com", DateOfBirth = new DateOnly(2000, 1, 28), IsActive = false },
                new User { Id = 10, Forename = "Johnny", Surname = "Blaze", Email = "jblaze@example.com", DateOfBirth = new DateOnly(2000, 1, 28), IsActive = true },
                new User { Id = 11, Forename = "Robin", Surname = "Feld", Email = "rfeld@example.com", DateOfBirth = new DateOnly(2000, 1, 28), IsActive = true },
            });

        /// <summary>
        /// Gets or sets the Users in the database.
        /// </summary>
        public DbSet<User>? Users { get; set; }

        /// <summary>
        /// Gets or sets the UserActionLogs in the database.
        /// </summary>
        public DbSet<UserActionLog> UserActionLogs { get; set; }

        /// <summary>
        /// Retrieves all entities of a given type.
        /// </summary>
        public async Task<IQueryable<TEntity>> GetAllAsync<TEntity>() where TEntity : class
        {
            return await Task.FromResult(Set<TEntity>().AsQueryable());
        }

        /// <summary>
        /// Retrieves all active or inactive entities of a given type.
        /// </summary>
        public async Task<IQueryable<TEntity>> GetActiveAsync<TEntity>(bool activeBool) where TEntity : class, IActiveEntity
        {
            return await Task.FromResult(Set<TEntity>().Where(e => e.IsActive == activeBool).AsQueryable());
        }

        /// <summary>
        /// Retrieves an entity by its ID.
        /// </summary>
        public async Task<TEntity> GetByIdAsync<TEntity>(long id) where TEntity : class, IActiveEntity
        {
            return await base.Set<TEntity>().FirstOrDefaultAsync(entity => entity.Id == id) ?? throw new ArgumentNullException();
        }

        /// <summary>
        /// Updates an entity in the database.
        /// </summary>
        public async Task UpdateAsync<TEntity>(TEntity entity) where TEntity : class
        {
            base.Update(entity);
            await SaveChangesAsync();
        }

        /// <summary>
        /// Deletes an entity from the database.
        /// </summary>
        public async Task DeleteAsync<TEntity>(TEntity entity) where TEntity : class
        {
            base.Remove(entity);
            await SaveChangesAsync();
        }

        /// <summary>
        /// Creates a new entity in the database.
        /// </summary>
        public async Task CreateAsync<TEntity>(TEntity entity) where TEntity : class
        {
            await Set<TEntity>().AddAsync(entity);
            await SaveChangesAsync();
        }
    }
}