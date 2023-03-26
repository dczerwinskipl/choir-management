using NEvo.Core;
using System.ComponentModel.DataAnnotations;

namespace NEvo.ValueObjects.PersonalData;

public record Email : IPersonalData<Email>
{
    [EmailAddress, MaxLength(255)]
    public string Value { get; init; }

    public Email(string value)
    {
        Value = Check.Annotations(this, nameof(Value), value);
    }

    private Email() { }

    public Email Anonimize() => new Email("anonymized@localhost");
}