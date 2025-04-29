using System;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System.Net;
using System.Net.Mail;
using Polly;
using Polly.Retry;

namespace LibraryManagementSystem.Data.Services
{
    public class EmailService : IEmailService
    {
        private readonly IConfiguration _configuration;
        private readonly ILogger<EmailService> _logger;
        private readonly IEmailTemplateService _templateService;
        private readonly BackupEmailService _backupEmailService;
        private readonly AsyncRetryPolicy _retryPolicy;
        private readonly string _smtpHost;
        private readonly int _smtpPort;
        private readonly string _smtpUsername;
        private readonly string _smtpPassword;
        private readonly string _fromEmail;
        private readonly bool _enableSsl;

        public EmailService(
            IConfiguration configuration,
            ILogger<EmailService> logger,
            IEmailTemplateService templateService,
            BackupEmailService backupEmailService)
        {
            _configuration = configuration;
            _logger = logger;
            _templateService = templateService;
            _backupEmailService = backupEmailService;

            _smtpHost = _configuration["Email:SmtpHost"];
            _smtpPort = int.Parse(_configuration["Email:SmtpPort"]);
            _smtpUsername = _configuration["Email:Username"];
            _smtpPassword = _configuration["Email:Password"];
            _fromEmail = _configuration["Email:FromAddress"];
            _enableSsl = bool.Parse(_configuration["Email:EnableSsl"] ?? "true");

            // Create retry policy
            _retryPolicy = Policy
                .Handle<SmtpException>()
                .Or<TimeoutException>()
                .WaitAndRetryAsync(3, retryAttempt => 
                    TimeSpan.FromSeconds(Math.Pow(2, retryAttempt)),
                    onRetry: (exception, timeSpan, retryCount, context) =>
                    {
                        _logger.LogWarning(
                            exception,
                            "Failed to send email (Attempt {RetryCount} of 3). Waiting {TimeSpan} seconds before retry.",
                            retryCount,
                            timeSpan.TotalSeconds);
                    }
                );
        }

        public async Task SendRentalConfirmationAsync(string toEmail, string purchaseId, string bookTitle, int copies, int days, decimal totalPrice)
        {
            _logger.LogInformation("Preparing to send rental confirmation email to {Email}", toEmail);

            try
            {
                await _retryPolicy.ExecuteAsync(async () =>
                {
                    var htmlBody = _templateService.GetRentalConfirmationTemplate(purchaseId, bookTitle, copies, days, totalPrice);

                    using (var client = new SmtpClient(_smtpHost, _smtpPort))
                    {
                        client.EnableSsl = _enableSsl;
                        client.Credentials = new NetworkCredential(_smtpUsername, _smtpPassword);
                        client.Timeout = 10000; // 10 seconds timeout

                        var message = new MailMessage
                        {
                            From = new MailAddress(_fromEmail, "Library System"),
                            Subject = $"Library Rental Confirmation - {purchaseId}",
                            Body = htmlBody,
                            IsBodyHtml = true
                        };
                        message.To.Add(toEmail);

                        _logger.LogInformation("Attempting to send email via SMTP...");
                        await client.SendMailAsync(message);
                        _logger.LogInformation("Email sent successfully to {Email}", toEmail);
                    }
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to send email via primary SMTP service. Attempting backup service...");

                try
                {
                    await _backupEmailService.SendRentalConfirmationAsync(toEmail, purchaseId, bookTitle, copies, days, totalPrice);
                }
                catch (Exception backupEx)
                {
                    _logger.LogError(backupEx, "Both primary and backup email services failed");
                    throw new AggregateException("Failed to send email through both primary and backup services", new[] { ex, backupEx });
                }
            }
        }
    }
} 