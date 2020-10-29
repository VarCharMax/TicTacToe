using Microsoft.AspNetCore.Mvc.Testing;
using System.Threading.Tasks;
using Xunit;

namespace TicTacToe.IntegrationTests
{
    public class IntegrationTests : IClassFixture<WebApplicationFactory<TicTacToe.Startup>>
    {
        private WebApplicationFactory<TicTacToe.Startup> _factory;

        public IntegrationTests(WebApplicationFactory<TicTacToe.Startup> factory)
        {
            _factory = factory;
        }

        [Fact]
        public async Task ShouldGetHomePageAsync()
        {
            var _client = _factory.CreateClient();

            var response = await _client.GetAsync("/");

            response.EnsureSuccessStatusCode();

            var responseString = await response.Content.ReadAsStringAsync();

            Assert.Contains("Welcome to the Tic-Tac-Toe Desktop Game!", responseString);
        }
    }
}
