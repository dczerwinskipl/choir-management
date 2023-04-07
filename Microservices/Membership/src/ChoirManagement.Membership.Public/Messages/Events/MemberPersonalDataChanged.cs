using ChoirManagement.Membership.Public.ValueObjects;
using NEvo.Core;
using NEvo.ValueObjects;

namespace ChoirManagement.Membership.Public.Messages.Events;

public record MemberPersonalDataChanged : MemberEvent
{
    public MemberPersonalData PersonalData { get; }

    public MemberPersonalDataChanged(MemberId memberId, MemberPersonalData personalData, ObjectId? source = null) : base(memberId, source)
    {
        PersonalData = Check.Null(personalData);
    }
}