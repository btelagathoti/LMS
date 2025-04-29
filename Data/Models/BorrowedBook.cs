using System.ComponentModel.DataAnnotations;

namespace LibraryManagementSystem.Data.Models
{
    public class BorrowedBook
    {
        public int Id { get; set; }

        [Required]
        public int BookId { get; set; }
        public Book Book { get; set; }

        [Required]
        public int MemberId { get; set; }
        public Member Member { get; set; }

        [Required]
        public DateTime BorrowDate { get; set; } = DateTime.UtcNow;

        [Required]
        public DateTime DueDate { get; set; }

        public DateTime? ReturnDate { get; set; }

        public bool IsReturned => ReturnDate.HasValue;

        public string Status => IsReturned ? "Returned" : 
            (DueDate < DateTime.UtcNow ? "Overdue" : "Borrowed");
    }
} 