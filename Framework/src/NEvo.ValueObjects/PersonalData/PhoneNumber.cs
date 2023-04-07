using System.ComponentModel.DataAnnotations;
using NEvo.Core;

namespace NEvo.ValueObjects.PersonalData;

public record PhoneNumber : IPersonalData<PhoneNumber>
{
    [Required(ErrorMessage = "Phone number is required")]
    [Phone(ErrorMessage = "Invalid phone number")]
    [MaxLength(32)]
    public string Value { get; init; }

    public PhoneNumber(string value)
    {
        Value = Check.Annotations(this, nameof(Value), value);
    }

    private PhoneNumber() { }

    public PhoneNumber Anonimize() => new PhoneNumber("000-000-000");
}
