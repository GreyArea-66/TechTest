using System;
using System.Linq;
using System.Threading.Tasks;
using UserManagement.Models;

namespace UserManagement.Data.Tests
{
    public class DataContextTests
    {
        [Fact]
        public async Task GetAll_WhenNewEntityAdded_MustIncludeNewEntity()
        {
            // Arrange: Initializes objects and sets the value of the data that is passed to the method under test.
            var context = CreateContext();

            var entity = new User
            {
                Forename = "Brand New",
                Surname = "User",
                Email = "brandnewuser@example.com",
                DateOfBirth = new DateOnly(2000, 1, 1)
            };
            await context.CreateAsync(entity);

            // Act: Invokes the method under test with the arranged parameters.
            var result = await context.GetAllAsync<User>();

            // Assert: Verifies that the action of the method under test behaves as expected.
            result
                .Should().Contain(s => s.Email == entity.Email)
                .Which.Should().BeEquivalentTo(entity);
        }

        [Fact]
        public async Task GetAll_WhenDeleted_MustNotIncludeDeletedEntity()
        {
            // Arrange: Initializes objects and sets the value of the data that is passed to the method under test.
            var context = CreateContext();
            var entity = (await context.GetAllAsync<User>()).First();
            await context.DeleteAsync(entity);

            // Act: Invokes the method under test with the arranged parameters.
            var result = await context.GetAllAsync<User>();

            // Assert: Verifies that the action of the method under test behaves as expected.
            result.Should().NotContain(s => s.Email == entity.Email);
        }

        private DataContext CreateContext() => new();
    }
}