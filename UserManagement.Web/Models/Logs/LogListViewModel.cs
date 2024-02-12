using System;

namespace UserManagement.Web.Models.Logs
{
    /// <summary>
    /// ViewModel for displaying a list of logs.
    /// </summary>
    public class LogListViewModel
    {
        /// <summary>
        /// List of logs to display.
        /// </summary>
        public List<LogListItemViewModel> Items { get; set; } = new();

        /// <summary>
        /// Current page number in pagination.
        /// </summary>
        public int CurrentPage { get; set; }

        /// <summary>
        /// Total number of pages in pagination.
        /// </summary>
        public int TotalPages { get; set; }

        /// <summary>
        /// Start date for filtering logs.
        /// </summary>
        public DateTime? startDateFilter { get; set; }

        /// <summary>
        /// End date for filtering logs.
        /// </summary>
        public DateTime? endDateFilter { get; set; }
    }

    /// <summary>
    /// ViewModel for displaying a single log item.
    /// </summary>
    public class LogListItemViewModel
    {
        /// <summary>
        /// ID of the log item.
        /// </summary>
        public long Id { get; set; }

        /// <summary>
        /// ID of the user associated with the log item.
        /// </summary>
        public long UserId { get; set; }

        /// <summary>
        /// Action associated with the log item.
        /// </summary>
        public string? Action { get; set; }

        /// <summary>
        /// Date of the action.
        /// </summary>
        public DateTime? ActionDate { get; set; }

        /// <summary>
        /// Details of the log item.
        /// </summary>
        public string? Details { get; set; }
    }
}