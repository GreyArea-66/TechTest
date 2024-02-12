using UserManagement.Models;
using UserManagement.Services.Domain.Interfaces;
using UserManagement.Web.Models.Users;
using UserManagement.WebMS.Controllers;
using UserManagement.Services.Interfaces;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;

namespace UserManagement.Data.Tests
{
    public class UserControllerTests
    {
        // Mock objects for IUserService and IUserActionLogSvc
        private readonly Mock<IUserService> _userService = new();
        private readonly Mock<IUserActionLogSvc> _userActionLogSvc = new();

        private readonly Mock<ILogger<UsersController>> _logger = new();

        [Fact]
        public async Task List_WhenServiceReturnsUsers_ModelMustContainUsers()
        {
            // Arrange
            // Create a controller and setup users
            var controller = CreateController();
            var users = SetupUsers();

            // Act
            // Call the List action method on the controller
            var result = await controller.List();
            var viewResult = result as ViewResult;
            viewResult.Should().NotBeNull();

            // Assert
            // Check that the model returned by the action is of type UserListViewModel
            // and that its Items property is equivalent to the users array
            var model = viewResult?.Model as UserListViewModel;
            model.Should().NotBeNull();

            if (model != null)
            {
                model.Items.Should().BeEquivalentTo(users);
            }
        }

        [Fact]
        public async Task List_WhenServiceReturnsNoUsers_ModelMustBeEmpty()
        {
            // Arrange
            var controller = CreateController();
            _userService.Setup(s => s.GetAllAsync()).ReturnsAsync(new User[0]); // Setup mock to return empty list
            var result = await controller.List(); // Add 'await' operator here
            var viewResult = result as ViewResult;
            viewResult.Should().NotBeNull();

            var model = viewResult?.Model as UserListViewModel;
            model.Should().NotBeNull();

            if (model != null)
            {
                model.Items.Should().BeEmpty();
            }
        }
        [Fact]
        public async Task List_WhenServiceThrowsException_ReturnsErrorView()
        {
            // Arrange
            var controller = CreateController();
            _userService.Setup(s => s.GetAllAsync()).ThrowsAsync(new Exception());

            // Act
            var result = await controller.List();

            // Assert
            var viewResult = result as ViewResult;
            viewResult.Should().NotBeNull();
            viewResult?.ViewName.Should().Be("Error");
        }
        private User[] SetupUsers(string forename = "Johnny", string surname = "User", string email = "juser@example.com", bool isActive = true)
        {
            // Create a users array with a single user
            var dateOfBirth = new DateOnly(1980, 1, 1);
            var users = new[]
            {
                new User
                {
                    Forename = forename,
                    Surname = surname,
                    Email = email,
                    DateOfBirth = dateOfBirth,
                    IsActive = isActive
                }
            };

            // Setup the user service to return the users array when GetAll is called
            _userService
                .Setup(s => s.GetAllAsync())
                .ReturnsAsync(users);

            return users;
        }

        private UsersController CreateController()
        {
            // Create a UsersController with the mock user service and user action log service
            return new(_userService.Object, _userActionLogSvc.Object, _logger.Object);
        }
    }
}