using Microsoft.Extensions.Options;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.DependencyInjection;
using NEvo.Core;
using static System.Linq.Enumerable;

namespace NEvo.Polling;

public class MessagePollerHostedService : IHostedService
{
    private IServiceProvider _serviceProvider;
    private IOptions<MessagePollerOptions> _options;
    private ConcurrentMultivalueDictionary<string, IMessagePoller> _topicPollers = new ConcurrentMultivalueDictionary<string, IMessagePoller>();

    public MessagePollerHostedService(IServiceProvider serviceProvider, IOptions<MessagePollerOptions> options)
    {
        _serviceProvider = serviceProvider;
        _options = options;
    }

    public async Task StartAsync(CancellationToken cancellationToken)
    {
        foreach (var topic in _options.Value.Topics)
        {
            var pollerTypeName = Check.NullOrEmpty(topic.PollerType ?? _options.Value.DefaultPollerType, $"No poller type defined for topic {topic.Name} or default poller.");
            var pollerType = AppDomain.CurrentDomain.GetAssemblies().Select(a => a.GetType(pollerTypeName)).FirstOrDefault(t => t is not null);
            // TODO: cache factory
            var factoryType = typeof(IMessagePollerFactory<>).MakeGenericType(Check.Null(pollerType, $"Type {pollerTypeName} not found in runtime."));
            var factory = (IMessagePollerFactory)_serviceProvider.GetRequiredService(factoryType);

            foreach (var _ in Range(1, topic.Threads))
            {
                var poller = await factory.CreatePollerAsync(topic.Name);
                _topicPollers.Add(topic.Name, poller);
                await poller.StartAsync(cancellationToken);
            }
        }
    }

    public async Task StopAsync(CancellationToken cancellationToken)
    {
        foreach (var poller in _topicPollers.Values.SelectMany(p => p))
        {
            await poller.StopAsync();
        }
    }
}
