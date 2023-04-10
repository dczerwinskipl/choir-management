using NEvo.Core;
using System.ComponentModel.DataAnnotations;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace NEvo.ValueObjects.PersonalData;

[JsonConverter(typeof(PhoneNumberConventer))]
[ValueObject(isSingleValue: true, defaultValue: "000-000-000", format: "phone")]
public record PhoneNumber : IPersonalData<PhoneNumber>
{
    [Required(ErrorMessage = "Phone number is required")]
    [Phone(ErrorMessage = "Invalid phone number")]
    [MaxLength(32)]
    public string Number { get; init; }

    [JsonConstructor]
    public PhoneNumber(string value)
    {
        Number = Check.Annotations(this, nameof(Number), value);
    }

    private PhoneNumber() { }

    public PhoneNumber Anonimize() => new("000-000-000");
}

public class PhoneNumberConventer : JsonConverter<PhoneNumber?>
{
    public override PhoneNumber? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        string value;
        try
        {
            value = reader.GetString() ?? string.Empty;
        }
        catch (Exception exc)
        {
            throw new ArgumentException($"Cannot parse {nameof(PhoneNumber)}. Invalid token on position ({reader.TokenStartIndex}): {exc.Message}", exc);
        }
        return !string.IsNullOrEmpty(value) ? new PhoneNumber(value) : null;
    }

    public override void Write(Utf8JsonWriter writer, PhoneNumber? value, JsonSerializerOptions options)
    {
        writer.WriteStringValue(value?.Number.ToString());
    }
}
