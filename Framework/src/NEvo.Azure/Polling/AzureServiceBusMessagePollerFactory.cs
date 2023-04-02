using Azure.Messaging.ServiceBus;
using Azure.Identity;
using NEvo.CQRS.Processing;
using Microsoft.Extensions.Options;
using NEvo.Core;
using Azure.Messaging.ServiceBus.Administration;
using NEvo.Polling;
using NEvo.Azure.Administrating;

namespace NEvo.Azure.Polling;

public class AzureServiceBusMessagePollerFactory : IMessagePollerFactory<AzureServiceBusMessagePoller>, IAsyncDisposable
{
    private ServiceBusClient _client;
    private readonly IOptions<AzureServiceBusClientData> _options;
    private readonly IAzureServiceBusAdministrator _azureAdministrator;
    private readonly IMessageProcessor _processor;
    private readonly INEvoAppDetailsProvider _nEvoAppDetailsProvider;
    private readonly NEvoAppDetails _nEvoAppDetails;
    private readonly string _subscriptionName;

    public AzureServiceBusMessagePollerFactory(IOptions<AzureServiceBusClientData> options, IAzureServiceBusAdministrator azureAdministrator, IMessageProcessor processor, INEvoAppDetailsProvider nEvoAppDetailsProvider)
    {
        _options = options;
        _azureAdministrator = azureAdministrator;
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
    }

    public async Task<AzureServiceBusMessagePoller> CreatePollerAsync(string topic)
    {
        await _azureAdministrator.EnsureTopicExistsAsync(topic);
        await _azureAdministrator.EnsureSubscriptionExistsAsync(topic, _subscriptionName);
        return new AzureServiceBusMessagePoller(_client, _processor, topic, _subscriptionName);
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
