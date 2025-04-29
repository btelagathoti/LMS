using LibraryManagementSystem.Data.Models;
using Microsoft.EntityFrameworkCore;

namespace LibraryManagementSystem.Data.Services
{
    public class MemberService : IMemberService
    {
        private readonly ApplicationDbContext _context;

        public MemberService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IList<Member>> GetAllMembersAsync()
        {
            return await _context.Members
                .Include(m => m.BorrowedBooks)
                .ThenInclude(bb => bb.Book)
                .ToListAsync();
        }

        public async Task<Member> GetMemberByIdAsync(int id)
        {
            return await _context.Members
                .Include(m => m.BorrowedBooks)
                .ThenInclude(bb => bb.Book)
                .FirstOrDefaultAsync(m => m.Id == id);
        }

        public async Task<Member> AddMemberAsync(Member member)
        {
            _context.Members.Add(member);
            await _context.SaveChangesAsync();
            return member;
        }

        public async Task<Member> UpdateMemberAsync(Member member)
        {
            member.UpdatedAt = DateTime.UtcNow;
            _context.Entry(member).State = EntityState.Modified;
            await _context.SaveChangesAsync();
            return member;
        }

        public async Task DeleteMemberAsync(int id)
        {
            var member = await _context.Members.FindAsync(id);
            if (member != null)
            {
                _context.Members.Remove(member);
                await _context.SaveChangesAsync();
            }
        }

        public async Task<bool> EmailExistsAsync(string email)
        {
            return await _context.Members.AnyAsync(m => m.Email == email);
        }

        public async Task<bool> MembershipNumberExistsAsync(string membershipNumber)
        {
            return await _context.Members.AnyAsync(m => m.MembershipNumber == membershipNumber);
        }
    }
}