using ChoirManagement.Membership.Domain.Aggregates;
using ChoirManagement.Membership.Public.ValueObjects;
using NEvo.Monads;

namespace ChoirManagement.Membership.Domain.Repositories
{
    public interface IMemberRepository
    {
        Task DeleteAsync(Member member);
        Maybe<Member> Get(MemberId memberId);
        Task<Maybe<Member>> GetAsync(MemberId memberId);
        void Save(Member member);
        Task SaveAsync(Member member);
    }
}