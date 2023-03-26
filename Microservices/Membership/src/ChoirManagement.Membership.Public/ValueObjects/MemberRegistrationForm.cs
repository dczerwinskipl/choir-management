using NEvo.ValueObjects.PersonalData;
using Newtonsoft.Json;

namespace ChoirManagement.Membership.Public.ValueObjects;

public record MemberRegistrationForm : MemberPersonalData
{
    [JsonConstructor]
    public MemberRegistrationForm(PESEL pesel, Name name, Address address, DateOnly birthDate, PhoneNumber phoneNumber, Email email) : base(pesel, name, address, birthDate, phoneNumber, email)
    {
    }

    protected MemberRegistrationForm(MemberPersonalData original) : base(original)
    {
    }
}