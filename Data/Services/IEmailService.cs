using System.Threading.Tasks;

namespace LibraryManagementSystem.Data.Services
{
    public interface IEmailService
    {
        Task SendRentalConfirmationAsync(string toEmail, string purchaseId, string bookTitle, int copies, int days, decimal totalPrice);
    }
} 