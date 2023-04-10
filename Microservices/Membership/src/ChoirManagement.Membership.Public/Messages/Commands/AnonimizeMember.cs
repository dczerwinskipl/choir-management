using ChoirManagement.Membership.Public.ValueObjects;
using NEvo.Core;
using NEvo.CQRS.Messaging.Commands;
using System.Text.Json.Serialization;

namespace ChoirManagement.Membership.Public.Messages.Commands;

public record AnonimizeMember : Command
{
    public MemberId MemberId { get; }

    [JsonConstructor]
    public AnonimizeMember(MemberId memberId)
    {
        MemberId = Check.Null(memberId);
    }
}


public record RegisterMember : Command
{
    public MemberId MemberId { get; }
    public MemberPersonalData PersonalData { get; }

    [JsonConstructor]
    public RegisterMember(MemberId memberId, MemberPersonalData personalData)
    {
        MemberId = Check.Null(memberId);
        PersonalData = Check.Null(personalData);
    }
}


public record DeleteMember : Command
{
    public MemberId MemberId { get; set; }

    [JsonConstructor]
    public DeleteMember(MemberId memberId)
    {
        MemberId = Check.Null(memberId);
    }
}