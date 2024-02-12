using System;

namespace UserManagement.Web.Models.Users
{
    /// <summary>
    /// ViewModel for displaying a list of users.
    /// </summary>
    public class UserListViewModel
    {
        /// <summary>
        /// List of users to display.
        /// </summary>
        public List<UserListItemViewModel> Items { get; set; } = new();
    }

    /// <summary>
    /// ViewModel for displaying a single user item.
    /// </summary>
    public class UserListItemViewModel
    {
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
}