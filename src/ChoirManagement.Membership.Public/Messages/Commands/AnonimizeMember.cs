using ChoirManagement.Membership.Public.ValueObjects;
using NEvo.Core;
using NEvo.Messaging.Commands;
using Newtonsoft.Json;

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
    public MemberRegistrationForm MemberRegistrationForm { get; }

    [JsonConstructor]
    public RegisterMember(MemberId memberId, MemberRegistrationForm memberRegistrationForm)
    {
        MemberId = Check.Null(memberId);
        MemberRegistrationForm = Check.Null(memberRegistrationForm);
    }
}