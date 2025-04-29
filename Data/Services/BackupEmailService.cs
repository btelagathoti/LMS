using System;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using SendGrid;
using SendGrid.Helpers.Mail;

namespace LibraryManagementSystem.Data.Services
{
    public class BackupEmailService : IEmailService
    {
        private readonly IConfiguration _configuration;
        private readonly ILogger<BackupEmailService> _logger;
        private readonly IEmailTemplateService _templateService;
        private readonly string _sendGridApiKey;

        public BackupEmailService(
            IConfiguration configuration,
            ILogger<BackupEmailService> logger,
            IEmailTemplateService templateService)
        {
            _configuration = configuration;
            _logger = logger;
            _templateService = templateService;
            _sendGridApiKey = _configuration["Email:SendGrid:ApiKey"];
        }

        public async Task SendRentalConfirmationAsync(string toEmail, string purchaseId, string bookTitle, int copies, int days, decimal totalPrice)
        {
            try
            {
                _logger.LogInformation("Attempting to send confirmation email via SendGrid to {Email}", toEmail);

                var client = new SendGridClient(_sendGridApiKey);
                var from = new EmailAddress(_configuration["Email:FromAddress"], "Library System");
                var to = new EmailAddress(toEmail);
                var subject = $"Library Rental Confirmation - {purchaseId}";
                var htmlContent = _templateService.GetRentalConfirmationTemplate(purchaseId, bookTitle, copies, days, totalPrice);

                var msg = MailHelper.CreateSingleEmail(from, to, subject, null, htmlContent);
                var response = await client.SendEmailAsync(msg);

                if (response.IsSuccessStatusCode)
                {
                    _logger.LogInformation("Successfully sent backup email via SendGrid to {Email}", toEmail);
                }
                else
                {
                    _logger.LogError("Failed to send backup email via SendGrid. Status Code: {StatusCode}", response.StatusCode);
                    throw new Exception($"SendGrid API returned status code: {response.StatusCode}");
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error sending backup email via SendGrid to {Email}", toEmail);
                throw;
            }
        }
    }
} 