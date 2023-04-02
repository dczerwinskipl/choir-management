using ChoirManagement.Membership.Public.ValueObjects;
using NEvo.Core;
using NEvo.CQRS.Messaging.Events;
using NEvo.ValueObjects;

namespace ChoirManagement.Membership.Public.Messages.Events;

public abstract record MemberEvent : Event
{
    public MemberId MemberId { get; }
    public MemberEvent(MemberId memberId, ObjectId? source = null) : base(source)
    {
        MemberId = Check.Null(memberId);
    }
}
