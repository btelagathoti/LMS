using System.ComponentModel.DataAnnotations;

namespace LibraryManagementSystem.Data.Models
{
    public class Member
    {
        public int Id { get; set; }

        [Required]
        [StringLength(20)]
        public string MembershipNumber { get; set; } = string.Empty;

        [Required]
        [StringLength(100)]
        public string FirstName { get; set; } = string.Empty;

        [Required]
        [StringLength(100)]
        public string LastName { get; set; } = string.Empty;

        public string FullName => $"{FirstName} {LastName}";

        [Required]
        [EmailAddress]
        [StringLength(100)]
        public string Email { get; set; } = string.Empty;

        [Phone]
        [StringLength(20)]
        public string? PhoneNumber { get; set; }

        [StringLength(200)]
        public string? Address { get; set; }

        [Required]
        public DateTime DateOfBirth { get; set; }

        [Required]
        public DateTime MembershipStartDate { get; set; } = DateTime.UtcNow;

        public DateTime? MembershipEndDate { get; set; }

        public DateTime? LastLoginDate { get; set; }

        public bool IsActive => !MembershipEndDate.HasValue || MembershipEndDate.Value > DateTime.UtcNow;

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;

        // Navigation property for borrowed books
        public ICollection<BorrowedBook> BorrowedBooks { get; set; } = new List<BorrowedBook>();
    }
} 