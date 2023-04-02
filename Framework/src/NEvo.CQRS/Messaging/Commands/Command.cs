using NEvo.Core;

namespace NEvo.CQRS.Messaging.Commands;

public abstract record Command : IMessage<Unit>
{
    public static MessageType MessageType => MessageType.Command;
    public DateTime CreatedAt { get; set; } = DateTime.Now;
}
