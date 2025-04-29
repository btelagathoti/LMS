using System.Text;
using Microsoft.Extensions.Configuration;

namespace LibraryManagementSystem.Data.Services
{
    public interface IEmailTemplateService
    {
        string GetRentalConfirmationTemplate(string purchaseId, string bookTitle, int copies, int days, decimal totalPrice);
        string GetFailedDeliveryTemplate(string purchaseId);
    }

    public class EmailTemplateService : IEmailTemplateService
    {
        private readonly IConfiguration _configuration;

        public EmailTemplateService(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public string GetRentalConfirmationTemplate(string purchaseId, string bookTitle, int copies, int days, decimal totalPrice)
        {
            var template = new StringBuilder();
            var libraryName = _configuration["LibraryInfo:Name"] ?? "Library Management System";
            var libraryAddress = _configuration["LibraryInfo:Address"] ?? "Main Street";
            var libraryPhone = _configuration["LibraryInfo:Phone"] ?? "123-456-7890";

            template.Append($@"
                <html>
                <head>
                    <style>
                        body {{ font-family: Arial, sans-serif; line-height: 1.6; color: #333; }}
                        .header {{ background-color: #4CAF50; color: white; padding: 20px; text-align: center; }}
                        .content {{ padding: 20px; }}
                        .details {{ background-color: #f9f9f9; padding: 15px; border-radius: 5px; margin: 20px 0; }}
                        .footer {{ background-color: #f1f1f1; padding: 15px; text-align: center; font-size: 12px; }}
                        .important {{ color: #e44d26; }}
                        .success {{ color: #4CAF50; }}
                    </style>
                </head>
                <body>
                    <div class='header'>
                        <h2>{libraryName}</h2>
                    </div>
                    <div class='content'>
                        <h3>Thank you for your rental!</h3>
                        <p>Your purchase ID is: <strong class='success'>{purchaseId}</strong></p>
                        
                        <div class='details'>
                            <h4>Rental Details:</h4>
                            <ul>
                                <li>Book: <strong>{bookTitle}</strong></li>
                                <li>Number of Copies: {copies}</li>
                                <li>Rental Period: {days} days</li>
                                <li>Total Price: ${totalPrice:F2}</li>
                            </ul>
                        </div>

                        <p class='important'>
                            Important: Please show this purchase ID at the library desk to collect your book(s).
                            Books must be collected within 24 hours of this rental.
                        </p>

                        <div class='details'>
                            <h4>Library Information:</h4>
                            <p>Address: {libraryAddress}</p>
                            <p>Phone: {libraryPhone}</p>
                            <p>Opening Hours: {_configuration["LibraryInfo:Hours"] ?? "9:00 AM - 5:00 PM"}</p>
                        </div>
                    </div>
                    <div class='footer'>
                        <p>This is an automated message. Please do not reply to this email.</p>
                        <p>© {DateTime.Now.Year} {libraryName}. All rights reserved.</p>
                    </div>
                </body>
                </html>");

            return template.ToString();
        }

        public string GetFailedDeliveryTemplate(string purchaseId)
        {
            return $@"
                <html>
                <body style='font-family: Arial, sans-serif;'>
                    <h2>Important: Your Recent Library Rental</h2>
                    <p>We noticed that our confirmation email for purchase ID <strong>{purchaseId}</strong> wasn't delivered successfully.</p>
                    <p>Please keep this purchase ID safe - you'll need it to collect your books.</p>
                    <hr>
                    <p style='color: #666;'>
                        If you need any assistance, please contact our support team.
                    </p>
                </body>
                </html>";
        }
    }
} 