using NEvo.Messaging;

namespace NEvo.Processing.Registering;

public record MessageHandlingOptions(bool AllowMultipleHandlers, bool RequireHandler, bool ReturnsValue)
{
    public static MessageHandlingOptions DefaultCommand = new(false, true, false);
    public static MessageHandlingOptions DefaultEvent = new(true, false, false);
    public static MessageHandlingOptions DefaultQuery = new(false, true, true);
    public static IDictionary<MessageType, MessageHandlingOptions> DefaultMessageHandlingOptions 
        = new Dictionary<MessageType, MessageHandlingOptions>() {
            { MessageType.Command, DefaultCommand },
            { MessageType.Event, DefaultEvent },
            { MessageType.Query, DefaultQuery }
        };
}
