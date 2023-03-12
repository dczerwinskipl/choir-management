namespace NEvo.Polling;

public interface IMessagePoller
{
    Task StartAsync(CancellationToken cancellationToken);
    Task StopAsync();
}
