using NEvo.ValueObjects;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace ChoirManagement.Membership.Public.ValueObjects;

[JsonConverter(typeof(MemberConventer))]
[ValueObject(isSingleValue: true, defaultValue: "00000000-0000-0000-0000-000000000000", format: "guid")]
public class MemberId : Identifier<MemberId, Guid>
{
    [JsonConstructor]
    private MemberId(Guid id) : base(id)
    {
    }

    private MemberId() : base() { }

    public static MemberId New() => new(Guid.NewGuid());

    public static MemberId New(Guid id) => new(id);

    public override MemberId Copy() => new(Id);

    public static implicit operator Guid(MemberId memberId) => memberId.Id;

    public static implicit operator MemberId(Guid id) => new(id);
}


public class MemberConventer : JsonConverter<MemberId?>
{
    public override MemberId? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        string stringId;
        try
        {
            stringId = reader.GetString() ?? string.Empty;
        }
        catch (Exception exc)
        {
            throw new ArgumentException($"Cannot parse {nameof(MemberId)}. Invalid token on position ({reader.TokenStartIndex}): {exc.Message}", exc);
        }

        return !string.IsNullOrEmpty(stringId) ? MemberId.New(Guid.Parse(stringId)) : null;
    }

    public override void Write(Utf8JsonWriter writer, MemberId? value, JsonSerializerOptions options)
    {
        writer.WriteStringValue(value?.Id.ToString());
    }
}