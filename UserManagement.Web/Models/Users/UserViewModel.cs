using System;

/// <summary>
/// ViewModel for displaying a user's details.
/// </summary>
public class UserViewModel
{
    /// <summary>
    /// List of action logs associated with the user.
    /// </summary>
    public IEnumerable<UserActionLog>? ActionLogs { get; set; }

    /// <summary>
    /// ID of the user.
    /// </summary>
    public long Id { get; set; }

    /// <summary>
    /// Forename of the user.
    /// </summary>
    public string? Forename { get; set; }

    /// <summary>
    /// Surname of the user.
    /// </summary>
    public string? Surname { get; set; }

    /// <summary>
    /// Email of the user.
    /// </summary>
    public string? Email { get; set; }

    /// <summary>
    /// Date of birth of the user.
    /// </summary>
    public DateOnly? DateOfBirth { get; set; }

    /// <summary>
    /// Indicates whether the user is active.
    /// </summary>
    public bool IsActive { get; set; }
}