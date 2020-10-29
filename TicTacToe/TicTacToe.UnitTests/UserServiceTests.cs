using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;
using TicTacToe.Data;
using TicTacToe.Models;
using TicTacToe.Services;
using Xunit;

namespace TicTacToe.UnitTests
{
    /*
    public class UserServiceTests
    {
        [Theory]
        [InlineData("test@test.com", "test", "test", "test123!")]
        [InlineData("test1@test.com", "test1", "test1", "test123!")]
        [InlineData("test2@test.com", "test2", "test2", "test123!")]
        public async Task ShouldAddUser(string email, string firstName, string lastName, string password)
        {
            var userModel = new UserModel {
                Email = email,
                FirstName = firstName,
                LastName = lastName,
                Password = password
            };

            var optionsBuilder = new DbContextOptionsBuilder<GameDbContext>();

            optionsBuilder.UseSqlServer(@"Server=(localdb)\MSSQLLocalDB;Database=TicTacToe;Trusted_Connection=true;MultipleActiveResultSets=true");

            var userService = new UserService(optionsBuilder.Options);
            var userAdded = await userService.RegisterUser(userModel);

            Assert.True(userAdded);
        }
    }
    */
}
