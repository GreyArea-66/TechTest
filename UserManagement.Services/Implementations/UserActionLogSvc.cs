using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using UserManagement.Data;
using UserManagement.Services.Interfaces;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Microsoft.EntityFrameworkCore;

/// <summary>
/// Service for managing user action logs.
/// </summary>
public class UserActionLogSvc : IUserActionLogSvc
{
    private readonly IDataContext _dataAccess;
    private readonly ILogger<UserActionLogSvc> _logger;

    /// <summary>
    /// Initializes a new instance of the UserActionLogSvc class.
    /// </summary>
    public UserActionLogSvc(IDataContext dataAccess, ILogger<UserActionLogSvc> logger)
    {
        _dataAccess = dataAccess;
        _logger = logger;
    }

    /// <summary>
    /// Logs an action performed by a user, comparing the properties of the original and updated objects and recording any changes.
    /// </summary>
    public async Task LogActionAsync(long userId, string action, object originalObject, object updatedObject)
    {
        // Determine the type of the input objects
        Type type = originalObject.GetType();

        // Extract the properties of the input objects
        PropertyInfo[] properties = type.GetProperties();

        // Initialize a StringBuilder to store the details of property changes
        var details = new StringBuilder();

        // Iterate over each property
        foreach (var property in properties)
        {
            // Get the value of the property in the original and updated objects
            object? originalValue = property.GetValue(originalObject);
            object? updatedValue = property.GetValue(updatedObject);

            // If both values are not null, compare them
            if (originalValue != null && updatedValue != null)
            {
                // If the property is a value type, use the Equals method for comparison
                if (property.PropertyType.IsValueType)
                {
                    if (!originalValue.Equals(updatedValue))
                    {
                        // If the values are different, record the change in the details string
                        details.AppendLine($"Field {property.Name} changed from {originalValue} to {updatedValue}");
                    }
                }
                else
                {
                    // If the property is a reference type, use the Equals method for comparison
                    if (!Equals(originalValue, updatedValue))
                    {
                        // If the values are different, record the change in the details string
                        details.AppendLine($"Field {property.Name} changed from {originalValue} to {updatedValue}");
                    }
                }
            }
        }

        // Create a new UserActionLog object with the recorded details
        var log = new UserActionLog
        {
            UserId = userId,
            Action = action,
            ActionDate = DateTime.UtcNow,
            Details = details.ToString()
        };

        // Add the new log to the database and save the changes
        _dataAccess.UserActionLogs.Add(log);
        await _dataAccess.SaveChangesAsync();
    }
    /// <summary>
    /// Retrieves all action logs for a specific user.
    /// </summary>
    public async Task<IEnumerable<UserActionLog>> GetActionLogsForUserAsync(long userId)
    {
        return await _dataAccess.UserActionLogs
            .Where(log => log.UserId == userId)
            .ToListAsync();
    }
    /// <summary>
    /// Retrieves an action log by its ID.
    /// </summary>
    public async Task<UserActionLog?> GetByIdAsync(long id)
    {
        return await _dataAccess.UserActionLogs
            .FirstOrDefaultAsync(log => log.Id == id);
    }
    /// <summary>
    /// Retrieves all action logs.
    /// </summary>
    public async Task<IEnumerable<UserActionLog>> GetAllAsync()
    {
        var logs = await _dataAccess.UserActionLogs.ToListAsync();
        _logger.LogInformation($"Retrieved {logs.Count} logs");
        return logs;
    }
}