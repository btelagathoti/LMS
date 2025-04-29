using LibraryManagementSystem.Data.Models;
using Microsoft.EntityFrameworkCore;

namespace LibraryManagementSystem.Data.Services
{
    public class BookService : IBookService
    {
        private readonly ApplicationDbContext _context;

        public BookService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IList<Book>> GetAllBooksAsync()
        {
            return await _context.Books
                .Include(b => b.BorrowedBooks)
                .ToListAsync();
        }

        public async Task<Book> GetBookByIdAsync(int id)
        {
            return await _context.Books
                .Include(b => b.BorrowedBooks)
                .FirstOrDefaultAsync(b => b.Id == id);
        }

        public async Task<Book> AddBookAsync(Book book)
        {
            book.AvailableCopies = book.TotalCopies;
            _context.Books.Add(book);
            await _context.SaveChangesAsync();
            return book;
        }

        public async Task<Book> UpdateBookAsync(Book book)
        {
            book.UpdatedAt = DateTime.UtcNow;
            _context.Entry(book).State = EntityState.Modified;
            await _context.SaveChangesAsync();
            return book;
        }

        public async Task DeleteBookAsync(int id)
        {
            var book = await _context.Books.FindAsync(id);
            if (book != null)
            {
                _context.Books.Remove(book);
                await _context.SaveChangesAsync();
            }
        }

        public async Task<bool> IsbnExistsAsync(string isbn)
        {
            return await _context.Books.AnyAsync(b => b.ISBN == isbn);
        }

        public async Task<List<Book>> SearchBooksAsync(string searchTerm)
        {
            if (string.IsNullOrWhiteSpace(searchTerm))
                return await _context.Books.ToListAsync();

            searchTerm = searchTerm.ToLower();
            return await _context.Books
                .Where(b => 
                    b.Title.ToLower().Contains(searchTerm) || 
                    b.Author.ToLower().Contains(searchTerm) || 
                    b.ISBN.ToLower().Contains(searchTerm) ||
                    b.Category.ToLower().Contains(searchTerm))
                .ToListAsync();
        }
    }
} 