using NEvo.Core;
using System.ComponentModel.DataAnnotations;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace NEvo.ValueObjects.PersonalData;

[JsonConverter(typeof(EmailConverter))]
[ValueObject(isSingleValue: true, defaultValue: "example@localhost", format: "email")]
public record Email : IPersonalData<Email>
{
    [EmailAddress, MaxLength(255)]
    public string Value { get; init; }

    public Email(string value)
    {
        Value = Check.Annotations(this, nameof(Value), value);
    }

    private Email() { }

    public Email Anonimize() => new("anonymized@localhost");
}

public class EmailConverter : JsonConverter<Email?>
{
    public override Email? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        string value;
        try
        {
            value = reader.GetString() ?? string.Empty;
        }
        catch (Exception exc)
        {
            throw new ArgumentException($"Cannot parse {nameof(Email)}. Invalid token on position ({reader.TokenStartIndex}): {exc.Message}", exc);
        }
        return !string.IsNullOrEmpty(value) ? new Email(value) : null;
    }

    public override void Write(Utf8JsonWriter writer, Email? value, JsonSerializerOptions options)
    {
        writer.WriteStringValue(value?.Value.ToString());
    }
}
