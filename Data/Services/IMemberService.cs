using LibraryManagementSystem.Data.Models;

namespace LibraryManagementSystem.Data.Services
{
    public interface IMemberService
    {
        Task<IList<Member>> GetAllMembersAsync();
        Task<Member> GetMemberByIdAsync(int id);
        Task<Member> AddMemberAsync(Member member);
        Task<Member> UpdateMemberAsync(Member member);
        Task DeleteMemberAsync(int id);
        Task<bool> EmailExistsAsync(string email);
        Task<bool> MembershipNumberExistsAsync(string membershipNumber);
    }
} 