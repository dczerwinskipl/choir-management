using Azure.Messaging.ServiceBus;
using Azure.Identity;
using NEvo.Processing;
using Microsoft.Extensions.Options;
using NEvo.Core;
using Azure.Messaging.ServiceBus.Administration;
using NEvo.Polling;

namespace NEvo.Azure.Polling;

public class AzureServiceBusMessagePollerFactory : IMessagePollerFactory<AzureServiceBusMessagePoller>, IAsyncDisposable
{
    private ServiceBusClient _client;
    private readonly ServiceBusAdministrationClient _administrationClient;
    private readonly IOptions<AzureServiceBusClientData> _options;
    private readonly IMessageProcessor _processor;
    private readonly INEvoAppDetailsProvider _nEvoAppDetailsProvider;
    private readonly NEvoAppDetails _nEvoAppDetails;
    private readonly string _subscriptionName;

    public AzureServiceBusMessagePollerFactory(IOptions<AzureServiceBusClientData> options, IMessageProcessor processor, INEvoAppDetailsProvider nEvoAppDetailsProvider)
    {
        _options = options;
        _processor = processor;
        _nEvoAppDetailsProvider = nEvoAppDetailsProvider;
        _nEvoAppDetails = _nEvoAppDetailsProvider.GetAppDetails();

        // TODO: maybe some naming policy?
        _subscriptionName = $"NEvo-{_nEvoAppDetails.AppName}";

        // TODO: external dependency
        _client = new ServiceBusClient(
           _options.Value.FullyQualifiedNamespace,
           new ClientSecretCredential(_options.Value.TenantId, _options.Value.ClientId, _options.Value.ClientSecret),
           new ServiceBusClientOptions { TransportType = ServiceBusTransportType.AmqpWebSockets });

        // TODO: external dependency
        _administrationClient = new ServiceBusAdministrationClient(_options.Value.FullyQualifiedNamespace,
           new ClientSecretCredential(_options.Value.TenantId, _options.Value.ClientId, _options.Value.ClientSecret),
           new ServiceBusAdministrationClientOptions { });
    }

    public async Task<AzureServiceBusMessagePoller> CreatePollerAsync(string topic)
    {
        await EnsureTopicExists(topic);
        await EnsureSubscriptionExists(topic, _subscriptionName);
        return new AzureServiceBusMessagePoller(_client, _processor, topic, _subscriptionName);
    }

    private async Task EnsureSubscriptionExists(string topic, string subscriptionName)
    {
        // TODO: policy, create or throw
        if (!await _administrationClient.SubscriptionExistsAsync(topic, subscriptionName))
        {
            // TODO: external dependency to create valid subscriptions
            await _administrationClient.CreateSubscriptionAsync(new CreateSubscriptionOptions(topic, subscriptionName)
            {
                RequiresSession = true
            });
        }
    }

    private async Task EnsureTopicExists(string topic)
    {
        //TODO: policy, create or throw
        if (!await _administrationClient.TopicExistsAsync(topic))
        {
            await _administrationClient.CreateTopicAsync(new CreateTopicOptions(topic)
            {
                EnablePartitioning = true,
                RequiresDuplicateDetection = true
            });
            await _administrationClient.CreateSubscriptionAsync(new CreateSubscriptionOptions(topic, "Preview")
            {
                RequiresSession = false
            });
            //throw new InvalidOperationException($"Cannot create poller if topic doesn't exists. Trying to create poller for: {topic}");
        }
    }

    public async ValueTask DisposeAsync()
    {
        if (_client is not null)
        {
            await _client.DisposeAsync();
            _client = null;
        }

        GC.SuppressFinalize(this);
    }
}
