using System.Collections.ObjectModel;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace NEvo.CQRS.Messaging;

[JsonConverter(typeof(MessageEnvelopeHeadersConverter))]
public class MessageEnvelopeHeaders : ReadOnlyDictionary<string, string>
{
    public MessageEnvelopeHeaders() : base(new Dictionary<string, string>()) { }

    [JsonConstructor]
    public MessageEnvelopeHeaders(IDictionary<string, string> dictionary) : base(dictionary)
    {
    }
}

public class MessageEnvelopeHeadersConverter : JsonConverter<MessageEnvelopeHeaders>
{
    public override MessageEnvelopeHeaders Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        var dictionary = JsonSerializer.Deserialize<Dictionary<string, string>>(ref reader, options);
        return new MessageEnvelopeHeaders(dictionary ?? new Dictionary<string, string>());
    }

    public override void Write(Utf8JsonWriter writer, MessageEnvelopeHeaders value, JsonSerializerOptions options)
    {
        JsonSerializer.Serialize(writer, value.ToDictionary(kv => kv.Key, kv => kv.Value), options);
    }
}