namespace NEvo.Messaging.Commands;

public abstract class Command : Message 
{
    public override sealed MessageType MessageType => MessageType.Command;
}
