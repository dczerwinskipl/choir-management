using Azure.Identity;
using Azure.Messaging.ServiceBus;
using Microsoft.Extensions.Options;
using NEvo.Azure.Administrating;
using NEvo.CQRS.Messaging;
using Newtonsoft.Json;

namespace NEvo.Azure.Publishing;

public interface IAzureServiceBusMessagePublisher
{
    void Publish<TMessage>(MessageEnvelope<TMessage> message, string topicName) where TMessage : IMessage => PublishAsync(message, topicName).ConfigureAwait(false).GetAwaiter().GetResult();
    Task PublishAsync<TMessage>(MessageEnvelope<TMessage> messsageEnvelope, string topicName) where TMessage : IMessage;

}

public class AzureServiceBusMessagePublisher : IAzureServiceBusMessagePublisher, IAsyncDisposable
{
    private readonly IOptions<AzureServiceBusClientData> _options;
    private readonly IAzureServiceBusAdministrator _azureServiceBusAdministrator;
    private readonly ServiceBusClient _client;

    public AzureServiceBusMessagePublisher(IOptions<AzureServiceBusClientData> options, IAzureServiceBusAdministrator azureServiceBusAdministrator)
    {
        _options = options;
        _azureServiceBusAdministrator = azureServiceBusAdministrator;

        var clientOptions = new ServiceBusClientOptions
        {
            TransportType = ServiceBusTransportType.AmqpWebSockets
        };
        _client = new ServiceBusClient(
           _options.Value.FullyQualifiedNamespace,
           new ClientSecretCredential(_options.Value.TenantId, _options.Value.ClientId, _options.Value.ClientSecret),
           clientOptions);
    }

    public async ValueTask DisposeAsync()
    {
        await _client.DisposeAsync();
    }

    public async Task PublishAsync<TMessage>(MessageEnvelope<TMessage> messageEnvelope, string topicName) where TMessage : IMessage
    {
        var serializedMessage = JsonConvert.SerializeObject(messageEnvelope.ToRawMessageEnvelope()); // TODO: Without ToRaw?

        await _azureServiceBusAdministrator.EnsureTopicExistsAsync(topicName);
        await using var sender = _client.CreateSender(topicName);

        var message = new ServiceBusMessage(serializedMessage)
        {
            PartitionKey = messageEnvelope.Partition,
            SessionId = messageEnvelope.Partition
        };

        await sender.SendMessageAsync(message);
    }
}
