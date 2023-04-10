using NEvo.Core;
using NEvo.ValueObjects.PersonalData;
using System.Text.Json.Serialization;

namespace ChoirManagement.Membership.Public.ValueObjects;

public record MemberPersonalData : IPersonalData<MemberPersonalData>
{
    public PESEL PESEL { get; init; }
    public Name Name { get; init; }
    public Address Address { get; init; }
    public DateOnly BirthDate { get; init; }
    public PhoneNumber PhoneNumber { get; init; }
    public Email Email { get; init; }

    [JsonConstructor]
    public MemberPersonalData(PESEL pesel, Name name, Address address, DateOnly birthDate, PhoneNumber phoneNumber, Email email)
    {
        PESEL = Check.Null(pesel);
        Name = Check.Null(name);
        Address = Check.Null(address);
        BirthDate = Check.Null(birthDate);
        PhoneNumber = Check.Null(phoneNumber);
        Email = Check.Null(email);
    }

    private MemberPersonalData() { }

    public MemberPersonalData Anonimize()
    {
        var pesel = PESEL.Anonimize();
        return new MemberPersonalData(
                pesel,
                Name.Anonimize(),
                Address.Anonimize(),
                pesel.GetBirthDate(),
                PhoneNumber.Anonimize(),
                Email.Anonimize()
        );
    }
}
