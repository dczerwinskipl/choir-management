using ChoirManagement.Membership.Public.ValueObjects;
using NEvo.ValueObjects;

namespace ChoirManagement.Membership.Public.Messages.Events;

public record MemberRegistered : MemberEvent
{
    public MemberRegistered(MemberId memberId, ObjectId? source = null) : base(memberId, source)
    {
    }
}
