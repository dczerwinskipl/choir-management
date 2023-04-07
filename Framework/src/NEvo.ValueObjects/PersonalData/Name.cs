using System.ComponentModel.DataAnnotations;
using NEvo.Core;
using Newtonsoft.Json;

namespace NEvo.ValueObjects.PersonalData;

public record Name : IPersonalData<Name>
{
    [MaxLength(32), Required]
    public string FirstName { get; init; }

    [MaxLength(32)]
    public string? MiddleName { get; init; }

    [MaxLength(32), Required]
    public string LastName { get; init; }

    [JsonConstructor]
    public Name(string firstName, string? middleName, string lastName)
    {
        FirstName = Check.NullOrEmpty(firstName);
        MiddleName = middleName;
        LastName = Check.NullOrEmpty(lastName);
        Check.Annotations(this);
    }

    private Name() { }

    public Name Anonimize() => new Name("anonymized", null, "anonymized");
}
