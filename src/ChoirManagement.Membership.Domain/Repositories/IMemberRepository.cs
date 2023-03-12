using ChoirManagement.Membership.Domain.Aggregates;
using ChoirManagement.Membership.Public.ValueObjects;
using NEvo.Core;

namespace ChoirManagement.Membership.Domain.Repositories
{
    public interface IMemberRepository
    { 
        Try<Member> Get(MemberId memberId);
        Task<Try<Member>> GetAsync(MemberId memberId);
        void Save(Member member);
        Task SaveAsync(Member member);
    }
}