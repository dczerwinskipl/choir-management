using ChoirManagement.Membership.Public.ValueObjects;
using NEvo.Core;
using System.Text.Json.Serialization;

namespace ChoirManagement.MembersRegistration.WebService.RegisterMembers.DTO;

public class RegisterInput
{
    public MemberId MemberId { get; set; }
    public MemberRegistrationForm Form { get; set; }

    [JsonConstructor]
    public RegisterInput(MemberId? memberId, MemberRegistrationForm form)
    {
        MemberId = memberId ?? MemberId.New();
        Form = Check.Null(form);
    }
}