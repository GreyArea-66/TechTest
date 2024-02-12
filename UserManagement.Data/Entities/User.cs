using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace UserManagement.Models
{
    /// <summary>
    /// Represents a user in the user management system.
    /// </summary>
    public class User : IActiveEntity
    {
        /// <summary>
        /// Gets or sets the unique identifier for the user.
        /// </summary>
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long Id { get; set; }

        /// <summary>
        /// Gets or sets the forename of the user.
        /// </summary>
        [Required(ErrorMessage = "Forename is required")]
        public string? Forename { get; set; } = default!;

        /// <summary>
        /// Gets or sets the surname of the user.
        /// </summary>
        [Required(ErrorMessage = "Surname is required")]
        public string? Surname { get; set; } = default!;

        /// <summary>
        /// Gets or sets the email address of the user.
        /// </summary>
        [Required(ErrorMessage = "Email is required")]
        [EmailAddress(ErrorMessage = "Invalid email address")]
        public string? Email { get; set; } = default!;

        /// <summary>
        /// Gets or sets the date of birth of the user.
        /// </summary>
        [Required(ErrorMessage = "Date of birth is required")]
        [PastDate(ErrorMessage = "Date of birth must be in the past")]
        public DateOnly? DateOfBirth { get; set; } = default!;

        /// <summary>
        /// Gets or sets a value indicating whether the user is active.
        /// </summary>
        public bool IsActive { get; set; }
    }
}