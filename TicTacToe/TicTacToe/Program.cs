using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;
using TicTacToe.Extensions;

namespace TicTacToe
{
    public class Program
    {
        public static void Main(string[] args)
        {
            CreateHostBuilder(args).Build().Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                    webBuilder.CaptureStartupErrors(true);
                    webBuilder.PreferHostingUrls(true);
                    webBuilder.UseUrls("http://localhost:5000");
                    webBuilder.ConfigureLogging((hostingcontext, logging) => {
                        logging.AddLoggingConfiguration(hostingcontext.Configuration);
                    });
                    webBuilder.UseIISIntegration();
                });
    }
}
