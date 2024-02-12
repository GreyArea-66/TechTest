using System.Linq;
using System.Threading.Tasks;
using UserManagement.Models;
using UserManagement.Services.Domain.Interfaces;
using UserManagement.Web.Models.Users;
using Microsoft.Extensions.Logging;
using Microsoft.EntityFrameworkCore;
using System;
using UserManagement.Services.Interfaces;

namespace UserManagement.WebMS.Controllers
{
    /// <summary>
    /// Controller for managing users.
    /// </summary>
    [Route("users")]
    public class UsersController : Controller
    {
        private readonly IUserService _userService;
        private readonly IUserActionLogSvc _userActionLogSvc;
        private readonly ILogger<UsersController> _logger;

        /// <summary>
        /// Constructor for UsersController.
        /// </summary>
        public UsersController(IUserService userService, IUserActionLogSvc userActionLogSvc, ILogger<UsersController> logger)
        {
            _userService = userService;
            _userActionLogSvc = userActionLogSvc;
            _logger = logger;
        }

        /// <summary>
        /// List all users.
        /// </summary>
        public async Task<IActionResult> List()
        {
            try
            {   //Fetch all users from servie
                var users = await _userService.GetAllAsync();
                //Map users to view model
                var items = users.Select(p => new UserListItemViewModel
                {
                    Id = p.Id,
                    Forename = p.Forename,
                    Surname = p.Surname,
                    Email = p.Email,
                    DateOfBirth = p.DateOfBirth,
                    IsActive = p.IsActive
                }).ToList();
                //Create model
                var model = new UserListViewModel
                {
                    Items = items
                };

                return View(model); // Ensure a ViewResult with a model is returned
            }
            catch (Exception)
            {
                return View("Error");
            }
        }
        /// <summary>
        /// List all active users.
        /// </summary>
        [HttpGet("active")]
        public async Task<IActionResult> Active()
        {
            var users = (await _userService.FilterByActiveAsync(true)).AsQueryable();
            return await GetUsers(users);
        }
        /// <summary>
        /// List all inactive users.
        /// </summary>
        [HttpGet("inactive")]
        public async Task<IActionResult> Inactive()
        {
            var users = (await _userService.FilterByActiveAsync(false)).AsQueryable();
            return await GetUsers(users);
        }
        /// <summary>
        /// Get users based on a query.
        /// </summary>
        private async Task<IActionResult> GetUsers(IQueryable<User> users)
        {
            try
            {
                var items = await users.Select(p => new UserListItemViewModel
                {
                    Id = p.Id,
                    Forename = p.Forename,
                    Surname = p.Surname,
                    Email = p.Email,
                    DateOfBirth = p.DateOfBirth,
                    IsActive = p.IsActive
                }).ToListAsync();

                var model = new UserListViewModel
                {
                    Items = items
                };

                return View("List", model);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting users");
                return View("Error");
            }
        }
        /// <summary>
        /// Add a new user.
        /// </summary>
        [HttpGet("add")]
        public IActionResult AddNewUser()
        {
            return View(new UserAddViewModel());
        }
        /// <summary>
        /// Add a new user (POST request).
        /// </summary>
        [HttpPost("add")]
        public async Task<IActionResult> AddNewUser(UserAddViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var user = new User
            {
                Forename = model.Forename,
                Surname = model.Surname,
                Email = model.Email,
                DateOfBirth = model.DateOfBirth,
                IsActive = model.IsActive
            };

            try
            {
                // Create the user
                await _userService.CreateAsync(user);
            }
            catch
            {
                // If there's an error, just rethrow it
                throw;
            }

            // If the user creation is successful, log the action
            await _userActionLogSvc.LogActionAsync(user.Id, "AddNewUser", null!, user);

            return RedirectToAction("List");
        }
        /// <summary>
        /// View a user.
        /// </summary>
        [HttpGet("viewuser")]
        public async Task<IActionResult> ViewUser([FromQuery(Name = "itemid")] long id)
        {
            var user = await _userService.GetByIdAsync(id);
            var actionLogs = _userActionLogSvc.GetActionLogsForUserAsync(id);

            var model = new UserViewModel
            {
                Id = user?.Id ?? 0,
                Forename = user?.Forename,
                Surname = user?.Surname,
                Email = user?.Email,
                DateOfBirth = user?.DateOfBirth,
                IsActive = user?.IsActive ?? false,
                ActionLogs = await actionLogs
            };

            return View(model);
        }

        /// <summary>
        /// Edit a user.
        /// </summary>
        [HttpGet("edituser")]
        public async Task<IActionResult> EditUser([FromQuery(Name = "itemid")] long id)
        {
            var user = await _userService.GetByIdAsync(id);
            if (user == null)
            {
                return NotFound();
            }

            return View(user);
        }
        /// <summary>
        /// Edit a user (POST request).
        /// </summary>
        [HttpPost("edituser")]
        public async Task<IActionResult> EditUser(User updatedUser)
        {
            if (!ModelState.IsValid)
            {
                return View(updatedUser);
            }

            // Get the original user
            var originalUser = await _userService.GetByIdAsync(updatedUser.Id);
            if (originalUser == null)
            {
                return NotFound();
            }

            try
            {
                // Log the action
                await _userActionLogSvc.LogActionAsync(updatedUser.Id, "EditUser", originalUser, updatedUser);

                // Update the original user with the updated user's values
                originalUser.Forename = updatedUser.Forename;
                originalUser.Surname = updatedUser.Surname;
                originalUser.Email = updatedUser.Email;
                originalUser.DateOfBirth = updatedUser.DateOfBirth;
                originalUser.IsActive = updatedUser.IsActive;

                // Update the user
                await _userService.UpdateAsync(originalUser);
            }
            catch
            {
                // If there's an error, just rethrow it
                throw;
            }

            return RedirectToAction("List");
        }
        /// <summary>
        /// Confirm deletion of a user.
        /// </summary>
        [HttpGet("confirmdeleteuser")]
        public async Task<IActionResult> ConfirmDeleteUser([FromQuery(Name = "itemid")] long id)
        {
            var user = await _userService.GetByIdAsync(id);
            if (user == null)
            {
                return NotFound();
            }

            return View(user);
        }
        /// <summary>
        /// Delete a user (POST request).
        /// </summary>
        [HttpPost("deleteuser")]
        public async Task<IActionResult> DeleteUser([FromForm] long id)
        {
            var user = await _userService.GetByIdAsync(id);
            if (user == null)
            {
                return NotFound();
            }

            await _userService.DeleteAsync(user);
            return RedirectToAction("List");
        }
        /// <summary>
        /// View log details.
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