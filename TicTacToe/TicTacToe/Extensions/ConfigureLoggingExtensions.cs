using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System.IO;
using TicTacToe.Logging;
using TicTacToe.Options;

namespace TicTacToe.Extensions
{
    public static class ConfigureLoggingExtensions
    {
        public static ILoggingBuilder AddLoggingConfiguration(this ILoggingBuilder loggingBuilder, IConfiguration configuration)
        {
            var loggingOptions = new LoggingOptions();
            configuration.GetSection("Logging").Bind(loggingOptions);

            foreach(var provider in loggingOptions.Providers)
            {
                switch (provider.Name.ToLower())
                {
                    case "console":
                        {
                            loggingBuilder.AddConsole();
                            break;
                        }
                    case "file":
                        {
                            string filePath = Path.Combine(Directory.GetCurrentDirectory(), "logs", $"TicTacToe_{System.DateTime.Now:ddMMyyHHmm}.log");

                            loggingBuilder.AddFile(filePath, (LogLevel)provider.LogLevel);

                            break;
                        }
                    case "azureappservices":
                        {
                            AzureAppServiceDiagnosticExtension.AddAzureWebAppDiagnostics(configuration, loggingBuilder);
                            break;
                        }
                    default:
                        {
                            break;
                        }
                }
            }

            return loggingBuilder;
        }
    }
}
