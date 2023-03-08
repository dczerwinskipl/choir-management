namespace NEvo.Polling;

public interface IMessagePollerFactory<TMessagePoller> : IMessagePollerFactory where TMessagePoller : IMessagePoller
{
    new Task<TMessagePoller> CreatePollerAsync(string topic);
    async Task<IMessagePoller> IMessagePollerFactory.CreatePollerAsync(string topic) => await CreatePollerAsync(topic);
}
