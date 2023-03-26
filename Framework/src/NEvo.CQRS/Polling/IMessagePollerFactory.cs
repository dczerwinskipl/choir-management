namespace NEvo.Polling;

public interface IMessagePollerFactory
{
    Task<IMessagePoller> CreatePollerAsync(string topic);
}
