using System;
using System.Collections.Generic;
using UserManagement.Data;
using UserManagement.Models;
using UserManagement.Services.Domain.Interfaces;
using System.Threading.Tasks;

namespace UserManagement.Services.Domain.Implementations
{
    /// <summary>
    /// Service for managing users.
    /// </summary>
    public class UserService : IUserService
    {
        private readonly IDataContext _dataAccess;

        /// <summary>
        /// Initializes a new instance of the UserService class.
        /// </summary>
        public UserService(IDataContext dataAccess) => _dataAccess = dataAccess;

        /// <summary>
        /// Retrieves all active or inactive users.
        /// </summary>
        public async Task<IEnumerable<User>> FilterByActiveAsync(bool isActive)
        => await _dataAccess.GetActiveAsync<User>(isActive);

        /// <summary>
        /// Retrieves all users.
        /// </summary>
        public async Task<IEnumerable<User>> GetAllAsync() => await _dataAccess.GetAllAsync<User>();

        /// <summary>
        /// Retrieves a user by its ID.
        /// </summary>
        public async Task<User> GetByIdAsync(long id) => await _dataAccess.GetByIdAsync<User>(id);

        /// <summary>
        /// Creates a new user.
        /// </summary>
        public async Task CreateAsync(User user)
        {
            if (user == null)
            {
                throw new ArgumentNullException(nameof(user));
            }

            await _dataAccess.CreateAsync(user);
        }
        /// <summary>
        /// Updates an existing user.
        /// </summary>
        public async Task UpdateAsync(User user)
        {
            if (user == null)
            {
                throw new ArgumentNullException(nameof(user));
            }

            await _dataAccess.UpdateAsync(user);
        }
        /// <summary>
        /// Deletes a user.
        /// </summary>
        public async Task DeleteAsync(User user)
        {
            if (user == null)
            {
                throw new ArgumentNullException(nameof(user));
            }

            await _dataAccess.DeleteAsync(user);
        }

    }
}