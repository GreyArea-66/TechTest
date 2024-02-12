using System.Collections.Generic;
using UserManagement.Models;
using System.Threading.Tasks;

namespace UserManagement.Services.Domain.Interfaces
{
    /// <summary>
    /// Interface for the User Service.
    /// </summary>
    public interface IUserService
    {
        /// <summary>
        /// Retrieves all active or inactive users.
        /// </summary>
        Task<IEnumerable<User>> FilterByActiveAsync(bool isActive);

        /// <summary>
        /// Retrieves all users.
        /// </summary>
        Task<IEnumerable<User>> GetAllAsync();

        /// <summary>
        /// Retrieves a user by its ID.
        /// </summary>
        Task<User> GetByIdAsync(long id);

        /// <summary>
        /// Creates a new user.
        /// </summary>
        Task CreateAsync(User user);

        /// <summary>
        /// Updates an existing user.
        /// </summary>
        Task UpdateAsync(User user);

        /// <summary>
        /// Deletes a user.
        /// </summary>
        Task DeleteAsync(User user);
    }
}