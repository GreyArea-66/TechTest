using System.Collections.Generic;
using System.Threading.Tasks;

namespace UserManagement.Services.Interfaces
{
    /// <summary>
    /// Interface for the User Action Log Service.
    /// </summary>
    public interface IUserActionLogSvc
    {
        /// <summary>
        /// Logs an action performed by a user.
        /// </summary>
        Task LogActionAsync(long userId, string action, object originalObject, object updatedObject);

        /// <summary>
        /// Retrieves all action logs for a specific user.
        /// </summary>
        Task<IEnumerable<UserActionLog>> GetActionLogsForUserAsync(long userId);

        /// <summary>
        /// Retrieves an action log by its ID.
        /// </summary>
        Task<UserActionLog?> GetByIdAsync(long id);

        /// <summary>
        /// Retrieves all action logs.
        /// </summary>
        Task<IEnumerable<UserActionLog>> GetAllAsync();
    }
}