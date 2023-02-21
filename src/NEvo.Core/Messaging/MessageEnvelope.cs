using NEvo.Core;

namespace NEvo.Messaging;

public class MessageEnvelope
{
    public Guid Id { get; set; }
    public string Payload { get; set; }
    public MessageEnvelopeHeaders Headers { get; set; }

    public MessageEnvelope(Guid id, string payload, MessageEnvelopeHeaders headers)
    {
        Id = Check.Default(id);
        Payload = Check.NullOrEmpty(payload);
        Headers = Check.Null(headers);
    }
}

public class MessageEnvelope<TMessage> where TMessage : IMessage
{
    public Guid Id { get; set; }
    public TMessage Payload { get; set; }
    public MessageEnvelopeHeaders Headers { get; set; }

    public MessageEnvelope(Guid id, TMessage payload, MessageEnvelopeHeaders headers)
    {
        Id = Check.Default(id);
        Payload = Check.Null(payload);
        Headers = Check.Null(headers);
    }
}