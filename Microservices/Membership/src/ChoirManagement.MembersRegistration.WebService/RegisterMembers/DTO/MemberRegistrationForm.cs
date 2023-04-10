using ChoirManagement.Membership.Public.ValueObjects;
using NEvo.Core;
using NEvo.ValueObjects;
using System.Text.Json.Serialization;

namespace ChoirManagement.MembersRegistration.WebService.RegisterMembers.DTO;

public class MemberRegistrationForm : ValueObject
{
    public UserIdentity Identity { get; set; }
    public MemberPersonalData PersonalData { get; set; }

    [JsonConstructor]
    public MemberRegistrationForm(UserIdentity identity, MemberPersonalData personalData)
    {
        Identity = Check.Null(identity);
        PersonalData = Check.Null(personalData);
    }

    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return Identity;
        yield return PersonalData;
    }
}