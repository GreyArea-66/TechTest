
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;


namespace UserManagement.Data
{
    /// <summary>
    /// Defines a contract for a data context.
    /// </summary>
    public interface IDataContext
    {
        /// <summary>
        /// Gets or sets the UserActionLogs in the database.
        /// </summary>
        DbSet<UserActionLog> UserActionLogs { get; set; }

        /// <summary>
        /// Saves all changes made in this context to the database.
        /// </summary>
        int SaveChanges();

        /// <summary>
        /// Asynchronously saves all changes made in this context to the database.
        /// </summary>
        Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);

        /// <summary>
        /// Retrieves all entities of a given type.
        /// </summary>
        Task<IQueryable<TEntity>> GetAllAsync<TEntity>() where TEntity : class;

        /// <summary>
        /// Retrieves all active or inactive entities of a given type.
        /// </summary>
        Task<IQueryable<TEntity>> GetActiveAsync<TEntity>(bool activeBool) where TEntity : class, IActiveEntity;

        /// <summary>
        /// Retrieves an entity by its ID.
        /// </summary>
        Task<TEntity> GetByIdAsync<TEntity>(long id) where TEntity : class, IActiveEntity;

        /// <summary>
        /// Creates a new entity in the database.
        /// </summary>
        Task CreateAsync<TEntity>(TEntity entity) where TEntity : class;

        /// <summary>
        /// Updates an entity in the database.
        /// </summary>
        Task UpdateAsync<TEntity>(TEntity entity) where TEntity : class;

        /// <summary>
        /// Deletes an entity from the database.
        /// </summary>
        Task DeleteAsync<TEntity>(TEntity entity) where TEntity : class;
    }
}