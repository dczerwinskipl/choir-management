using ChoirManagement.Membership.Public.ValueObjects;
using NEvo.ValueObjects;

namespace ChoirManagement.Membership.Public.Messages.Events;

public record MemberAnonimzed : MemberEvent
{
    public MemberAnonimzed(MemberId memberId, ObjectId? source = null) : base(memberId, source)
    {
    }
}
