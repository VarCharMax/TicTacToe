using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace TicTacToe.Extensions
{
    public class AzureAppServiceDiagnosticExtension
    {
        public static void AddAzureWebAppDiagnostics(IConfiguration configuration, ILoggingBuilder loggingBuilder)
        {
            loggingBuilder.AddAzureWebAppDiagnostics();
        }
    }
}
