namespace NEvo.CQRS.Publishing;

public class MessagePublisherConfiguration
{
    public string Type { get; set; }
    public MessagePublisherOutbox Outbox { get; set; } = MessagePublisherOutbox.None;

    /// <summary>
    /// Nie wiem czy nie wywalić?
    /// </summary>
    public enum MessagePublisherOutbox
    {
        None = 0,
        InMemory = 1,
        Persistent = 2
    }
}
