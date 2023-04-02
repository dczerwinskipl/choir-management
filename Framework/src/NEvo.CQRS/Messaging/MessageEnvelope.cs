using NEvo.Core;
using Newtonsoft.Json;

namespace NEvo.CQRS.Messaging;

public class MessageEnvelope
{
    public Guid Id { get; set; }
    public string Payload { get; set; }
    public MessageEnvelopeHeaders Headers { get; set; }
    public string MessageType { get; set; }

    public MessageEnvelope(Guid id, string payload, string messageType) : this(id, payload, messageType, new MessageEnvelopeHeaders()) { }

    [JsonConstructor]
    public MessageEnvelope(Guid id, string payload, string messageType, MessageEnvelopeHeaders headers)
    {
        Id = Check.Default(id);
        Payload = Check.NullOrEmpty(payload);
        MessageType = Check.Null(messageType);
        Headers = Check.Null(headers);
    }
}

public class MessageEnvelope<TMessage> where TMessage : IMessage
{
    public Guid Id { get; set; }
    public TMessage Payload { get; set; }
    public MessageEnvelopeHeaders Headers { get; set; }
    public string MessageType { get; set; }
    public string? Partition { get; }

    public MessageEnvelope(Guid id, TMessage payload, string messageType) : this(id, payload, messageType, null) { }

    public MessageEnvelope(Guid id, TMessage payload, string messageType, string? partition) : this(id, payload, messageType, partition, new MessageEnvelopeHeaders()) { }

    [JsonConstructor]
    public MessageEnvelope(Guid id, TMessage payload, string messageType, string? partition, MessageEnvelopeHeaders headers)
    {
        Id = Check.Default(id);
        Payload = Check.Null(payload);
        MessageType = Check.Null(messageType);
        Headers = Check.Null(headers);
        Partition = partition;
    }

    public MessageEnvelope ToRawMessageEnvelope() => new MessageEnvelope(Id, JsonConvert.SerializeObject(Payload), MessageType, Headers); //TODO: remove dep to Newtonsoft
}