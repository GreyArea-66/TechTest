using System;
using System.ComponentModel.DataAnnotations;

/// <summary>
/// ViewModel for adding a new user.
/// </summary>
public class UserAddViewModel
{
    /// <summary>
    /// Forename of the user. This field is required and cannot be longer than 50 characters.
    /// </summary>
    [Required(ErrorMessage = "Forename is required.")]
    [StringLength(50, ErrorMessage = "Forename cannot be longer than 50 characters.")]
    public string? Forename { get; set; }

    /// <summary>
    /// Surname of the user. This field is required and cannot be longer than 50 characters.
    /// </summary>
    [Required(ErrorMessage = "Surname is required.")]
    [StringLength(50, ErrorMessage = "Surname cannot be longer than 50 characters.")]
    public string? Surname { get; set; }

    /// <summary>
    /// Email of the user. This field is required and must be a valid email address.
    /// </summary>
    [Required(ErrorMessage = "Email is required.")]
    [EmailAddress(ErrorMessage = "Invalid Email Address.")]
    public string? Email { get; set; }

    /// <summary>
    /// Date of birth of the user. This field is required and must be a past date.
    /// </summary>
    [Required(ErrorMessage = "Date of Birth is required.")]
    [PastDate(ErrorMessage = "Date of Birth must be in the past.")]
    public DateOnly? DateOfBirth { get; set; }

    /// <summary>
    /// Indicates whether the user is active.
    /// </summary>
    public bool IsActive { get; set; }
}