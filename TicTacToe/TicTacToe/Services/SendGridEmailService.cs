using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using SendGrid.Helpers.Mail;
using System.Threading.Tasks;
using TicTacToe.Options;

namespace TicTacToe.Services
{
    public class SendGridEmailService : IEmailService
    {
        private EmailServiceOptions _emailServiceOptions;
        private ILogger<EmailService> _logger;

        public SendGridEmailService(IOptions<EmailServiceOptions> emailServiceOptions, ILogger<EmailService> logger)
        {
            _emailServiceOptions = emailServiceOptions.Value;
            _logger = logger;
        }

        public Task SendEmail(string emailTo, string subject, string message)
        {
            _logger.LogInformation($"##Start## Sending email via SendGrid to: {emailTo} subject: {subject} message: {message}");

            var client = new SendGrid.SendGridClient(_emailServiceOptions.RemoteServerAPI);

            var sendGridMessage = new SendGridMessage {
                From = new EmailAddress(_emailServiceOptions.UserId)
            };

            sendGridMessage.AddTo(emailTo);
            sendGridMessage.Subject = subject;
            sendGridMessage.HtmlContent = message;
            client.SendEmailAsync(sendGridMessage);

            return Task.CompletedTask;
        }
    }
}
