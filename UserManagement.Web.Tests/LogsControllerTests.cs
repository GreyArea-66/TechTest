using UserManagement.WebMS.Controllers;
using UserManagement.Services.Interfaces;
using UserManagement.Web.Models.Logs;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace UserManagement.Data.Tests
{
    public class LogsControllerTests
    {
        private readonly Mock<IUserActionLogSvc> _userActionLogSvc = new();
        private readonly Mock<ILogger<LogsController>> _logger = new();

        [Fact]
        public async Task List_WhenCalled_ReturnsCorrectViewModel()
        {
            var controller = CreateController();
            _userActionLogSvc.Setup(s => s.GetAllAsync()).ReturnsAsync(new List<UserActionLog>());

            var result = await controller.List(null, null!, null, null) as ViewResult;

            var model = result.Model as LogListViewModel;
            model.Should().NotBeNull();
        }

        [Fact]
        public async Task LogDetails_WhenCalledWithValidId_ReturnsCorrectView()
        {
            var controller = CreateController();
            _userActionLogSvc.Setup(s => s.GetByIdAsync(It.IsAny<long>())).ReturnsAsync(new UserActionLog());

            var result = await controller.LogDetails(1) as ViewResult;

            result.Should().NotBeNull();
        }

        [Fact]
        public async Task List_WhenCalledWithUserId_FiltersByUserId()
        {
            var controller = CreateController();
            var logs = new List<UserActionLog>
        {
            new UserActionLog { UserId = 1 },
            new UserActionLog { UserId = 2 }
        };
            _userActionLogSvc.Setup(s => s.GetAllAsync()).ReturnsAsync(logs);

            var result = await controller.List(1, null!, null, null) as ViewResult;

            var model = result.Model as LogListViewModel;
            model?.Items.Should().OnlyContain(i => i.UserId == 1);
        }

        [Fact]
        public async Task List_WhenCalledWithDateRange_FiltersByDate()
        {
            var controller = CreateController();
            var logs = new List<UserActionLog>
        {
            new UserActionLog { ActionDate = new DateTime(2022, 1, 1) },
            new UserActionLog { ActionDate = new DateTime(2022, 2, 1) }
        };
            _userActionLogSvc.Setup(s => s.GetAllAsync()).ReturnsAsync(logs);

            var result = await controller.List(null, null!, new DateTime(2022, 1, 1), new DateTime(2022, 1, 31)) as ViewResult;

            var model = result.Model as LogListViewModel;
            model?.Items.Should().OnlyContain(i => i.ActionDate.HasValue && i.ActionDate.Value == new DateTime(2022, 1, 1));
        }

        [Fact]
        public async Task LogDetails_WhenCalledWithInvalidId_ReturnsNotFound()
        {
            var controller = CreateController();
            _userActionLogSvc.Setup(s => s.GetByIdAsync(It.IsAny<long>())).ReturnsAsync((UserActionLog?)null);

            var result = await controller.LogDetails(1);

            result.Should().BeOfType<NotFoundResult>();
        }

        private LogsController CreateController()
        {
            return new(_userActionLogSvc.Object, _logger.Object);
        }
    }
}