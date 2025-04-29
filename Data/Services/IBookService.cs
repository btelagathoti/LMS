using LibraryManagementSystem.Data.Models;

namespace LibraryManagementSystem.Data.Services
{
    public interface IBookService
    {
        Task<IList<Book>> GetAllBooksAsync();
        Task<Book> GetBookByIdAsync(int id);
        Task<Book> AddBookAsync(Book book);
        Task<Book> UpdateBookAsync(Book book);
        Task DeleteBookAsync(int id);
        Task<bool> IsbnExistsAsync(string isbn);
        Task<List<Book>> SearchBooksAsync(string searchTerm);
    }
} 