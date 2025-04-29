using LibraryManagementSystem.Data.Models;
using Microsoft.EntityFrameworkCore;

namespace LibraryManagementSystem.Data
{
    public static class DbInitializer
    {
        public static async Task InitializeAsync(ApplicationDbContext context)
        {
            // Ensure database is created
            await context.Database.EnsureCreatedAsync();

            // Check if we already have data
            if (await context.Books.AnyAsync() || await context.Members.AnyAsync())
            {
                return; // Database has been seeded
            }

            // Add sample members
            var members = new[]
            {
                new Member
                {
                    MembershipNumber = "M001",
                    FirstName = "John",
                    LastName = "Doe",
                    Email = "john.doe@example.com",
                    PhoneNumber = "123-456-7890",
                    Address = "123 Main St",
                    DateOfBirth = new DateTime(1990, 1, 1),
                    MembershipStartDate = DateTime.UtcNow
                },
                new Member
                {
                    MembershipNumber = "M002",
                    FirstName = "Jane",
                    LastName = "Smith",
                    Email = "jane.smith@example.com",
                    PhoneNumber = "098-765-4321",
                    Address = "456 Oak Ave",
                    DateOfBirth = new DateTime(1985, 5, 15),
                    MembershipStartDate = DateTime.UtcNow
                }
            };

            await context.Members.AddRangeAsync(members);
            await context.SaveChangesAsync();

            // Add sample books
            var books = new[]
            {
                new Book
                {
                    ISBN = "9780132350884",
                    Title = "Clean Code",
                    Author = "Robert C. Martin",
                    Category = "Technology",
                    Description = "A handbook of agile software craftsmanship",
                    TotalCopies = 5,
                    AvailableCopies = 5
                },
                new Book
                {
                    ISBN = "9780547928227",
                    Title = "The Hobbit",
                    Author = "J.R.R. Tolkien",
                    Category = "Fiction",
                    Description = "A fantasy novel about Bilbo Baggins' journey",
                    TotalCopies = 3,
                    AvailableCopies = 3
                },
                new Book
                {
                    ISBN = "9780439708180",
                    Title = "Harry Potter and the Sorcerer's Stone",
                    Author = "J.K. Rowling",
                    Category = "Fiction",
                    Description = "The first book in the Harry Potter series",
                    TotalCopies = 4,
                    AvailableCopies = 4
                },
                new Book
                {
                    ISBN = "9781449331818",
                    Title = "Learning JavaScript Design Patterns",
                    Author = "Addy Osmani",
                    Category = "Technology",
                    Description = "A comprehensive guide to JavaScript design patterns",
                    TotalCopies = 2,
                    AvailableCopies = 2
                },
                new Book
                {
                    ISBN = "9780743273565",
                    Title = "The Great Gatsby",
                    Author = "F. Scott Fitzgerald",
                    Category = "Fiction",
                    Description = "A story of the mysteriously wealthy Jay Gatsby",
                    TotalCopies = 3,
                    AvailableCopies = 3
                },
                new Book
                {
                    ISBN = "9780307474278",
                    Title = "Born to Run",
                    Author = "Christopher McDougall",
                    Category = "Non-Fiction",
                    Description = "A hidden tribe, superathletes, and the greatest race the world has never seen",
                    TotalCopies = 2,
                    AvailableCopies = 2
                },
                new Book
                {
                    ISBN = "9781982181284",
                    Title = "A Brief History of Time",
                    Author = "Stephen Hawking",
                    Category = "Science",
                    Description = "From the Big Bang to Black Holes",
                    TotalCopies = 3,
                    AvailableCopies = 3
                },
                new Book
                {
                    ISBN = "9780316769488",
                    Title = "The Catcher in the Rye",
                    Author = "J.D. Salinger",
                    Category = "Fiction",
                    Description = "A coming-of-age novel by J.D. Salinger",
                    TotalCopies = 4,
                    AvailableCopies = 4
                }
            };

            await context.Books.AddRangeAsync(books);
            await context.SaveChangesAsync();

            // Add some sample borrowed books
            var borrowedBooks = new[]
            {
                new BorrowedBook
                {
                    BookId = books[0].Id,
                    MemberId = members[0].Id,
                    BorrowDate = DateTime.UtcNow.AddDays(-10),
                    DueDate = DateTime.UtcNow.AddDays(4)
                },
                new BorrowedBook
                {
                    BookId = books[1].Id,
                    MemberId = members[1].Id,
                    BorrowDate = DateTime.UtcNow.AddDays(-5),
                    DueDate = DateTime.UtcNow.AddDays(9)
                }
            };

            await context.BorrowedBooks.AddRangeAsync(borrowedBooks);

            // Update available copies for borrowed books
            books[0].AvailableCopies--;
            books[1].AvailableCopies--;

            await context.SaveChangesAsync();
        }
    }
} 