using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using UserManagement.Services.Interfaces;
using UserManagement.Web.Models.Logs;

namespace UserManagement.WebMS.Controllers
{
    /// <summary>
    /// Controller for managing user action logs.
    /// </summary>
    [Route("logs")]
    public class LogsController : Controller
    {
        private readonly IUserActionLogSvc _userActionLogSvc;
        private readonly ILogger<LogsController> _logger;

        /// <summary>
        /// Initializes a new instance of the LogsController class.
        /// </summary>
        public LogsController(IUserActionLogSvc userActionLogSvc, ILogger<LogsController> logger)
        {
            _userActionLogSvc = userActionLogSvc;
            _logger = logger;
        }

        /// <summary>
        /// Retrieves a list of user action logs.
        /// </summary>
        [HttpGet]
        public async Task<ViewResult> List(long? userId, string logAction, DateTime? startDate, DateTime? endDate, int page = 1, int pageSize = 10)
        {
            // Get all logs from the service
            var allLogs = (await _userActionLogSvc.GetAllAsync()).ToList();

            // Get distinct actions from the logs
            var availableActions = allLogs.Select(p => p.Action).Distinct().ToList();

            // Map logs to view model
            var items = allLogs.Select(p => new LogListItemViewModel
            {
                Id = p.Id,
                UserId = p.UserId,
                Action = p.Action,
                ActionDate = p.ActionDate,
                Details = p.Details
            });

            // Filter by user ID if provided
            if (userId.HasValue)
            {
                items = items.Where(p => p.UserId == userId);
            }

            // Filter by action if provided
            if (!string.IsNullOrEmpty(logAction))
            {
                items = items.Where(p => p.Action == logAction);
            }

            // Filter by date range if provided
            if (startDate.HasValue || endDate.HasValue)
            {
                if (startDate.HasValue)
                {
                    DateTime startDateUtc = startDate.Value.ToUniversalTime();
                    items = items.Where(p => p.ActionDate >= startDateUtc);
                }
                if (endDate.HasValue)
                {
                    DateTime endDateUtc = endDate.Value.ToUniversalTime();
                    items = items.Where(p => p.ActionDate <= endDateUtc);
                }
            }

            // Set date filters for the view
            ViewBag.startDateFilter = startDate.HasValue ? startDate.Value.ToString("yyyy-MM-ddTHH:mm") : string.Empty;
            ViewBag.endDateFilter = endDate.HasValue ? endDate.Value.ToString("yyyy-MM-ddTHH:mm") : string.Empty;

            // Get total items after filtering
            var totalItems = items.Count();

            // Apply pagination
            items = items
                .Skip((page - 1) * pageSize)
                .Take(pageSize);

            // Prepare the view model for the view
            var model = new LogListViewModel
            {
                Items = items.ToList(),
                CurrentPage = page,
                TotalPages = (int)Math.Ceiling((double)totalItems / pageSize),
            };

            // Set available actions for the view
            ViewBag.AvailableActions = availableActions;

            // Return the view with the model
            return View("LogsList", model);
        }
        /// <summary>
        /// Retrieves the details of a specific user action log.
        /// </summary>
        [HttpGet("logdetails")]
        public async Task<IActionResult> LogDetails([FromQuery] long id)
        {
            var log = await _userActionLogSvc.GetByIdAsync(id);
            if (log == null)
            {
                return NotFound();
            }
            return View(log);
        }
    }
}