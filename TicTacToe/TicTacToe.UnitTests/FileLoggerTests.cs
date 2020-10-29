using Microsoft.Extensions.Logging;
using System.IO;
using TicTacToe.Logging;
using Xunit;

namespace TicTacToe.UnitTests
{
    public class FileLoggerTests
    {
        [Fact]
        public void ShouldCreateALogFile()
        {
            var fileLogger = new FileLogger("Test", (category, level) => true, Path.Combine(Directory.GetCurrentDirectory(), "testlog.log"));

            var isEnabled = fileLogger.IsEnabled(LogLevel.Information);

            Assert.True(isEnabled);
        }
    }
}
