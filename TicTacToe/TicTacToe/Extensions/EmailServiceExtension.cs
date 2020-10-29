using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using TicTacToe.Options;
using TicTacToe.Services;

namespace TicTacToe.Extensions
{
    public static class EmailServiceExtension
    {
        public static IServiceCollection AddEmailService(this IServiceCollection services, 
            IWebHostEnvironment hostingEnvironment, IConfiguration configuration)
        {
            services.Configure<EmailServiceOptions>(configuration.GetSection("Email"));

            if (hostingEnvironment.IsDevelopment() || hostingEnvironment.IsStaging())
            {
                services.AddSingleton<IEmailService, EmailService>();
            }
            else
            {
                services.AddSingleton<IEmailService, SendGridEmailService>();
            }

            return services;
        }
    }
}
