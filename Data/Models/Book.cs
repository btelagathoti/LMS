using System.ComponentModel.DataAnnotations;

namespace LibraryManagementSystem.Data.Models
{
    public class Book
    {
        public int Id { get; set; }

        [Required]
        [StringLength(13)]
        public string ISBN { get; set; } = string.Empty;

        [Required]
        [StringLength(200)]
        public string Title { get; set; } = string.Empty;

        [Required]
        [StringLength(200)]
        public string Author { get; set; } = string.Empty;

        [Required]
        [StringLength(100)]
        public string Category { get; set; } = string.Empty;

        [StringLength(2000)]
        public string? Description { get; set; }

        [Required]
        [Range(0, int.MaxValue)]
        public int TotalCopies { get; set; }

        [Required]
        [Range(0, int.MaxValue)]
        public int AvailableCopies { get; set; }

        public bool IsAvailable => AvailableCopies > 0;

        [StringLength(200)]
        public string? Publisher { get; set; }

        public DateTime? PublicationDate { get; set; }

        [StringLength(100)]
        public string? Location { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        
        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;

        // Navigation property for borrowed books
        public virtual ICollection<BorrowedBook> BorrowedBooks { get; set; } = new List<BorrowedBook>();
    }
} 