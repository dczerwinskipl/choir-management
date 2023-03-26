using Azure.Messaging.ServiceBus;
using Azure.Identity;
using NEvo.Messaging;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using NEvo.Publishing;
using Azure.Messaging.ServiceBus.Administration;

namespace NEvo.Azure.Publishing;
public class AzureServiceBusMessagePublisher : IMessagePublisher, IAsyncDisposable
{
    private readonly IOptions<AzureServiceBusClientData> _options;
    private readonly ITopicProvider _topicProvider;
    private readonly ServiceBusClient _client;
    private readonly ServiceBusAdministrationClient _administrationClient;
    private readonly HashSet<string> _topics = new HashSet<string>();
    static SemaphoreSlim _topicsSemaphoreSlim = new SemaphoreSlim(1, 1);
    public AzureServiceBusMessagePublisher(IOptions<AzureServiceBusClientData> options, ITopicProvider topicProvider)
    {
        _options = options;
        _topicProvider = topicProvider;

        var clientOptions = new ServiceBusClientOptions
        {
            TransportType = ServiceBusTransportType.AmqpWebSockets
        };
        _client = new ServiceBusClient(
           _options.Value.FullyQualifiedNamespace,
           new ClientSecretCredential(_options.Value.TenantId, _options.Value.ClientId, _options.Value.ClientSecret),
           clientOptions);

        _administrationClient = new ServiceBusAdministrationClient(_options.Value.FullyQualifiedNamespace,
           new ClientSecretCredential(_options.Value.TenantId, _options.Value.ClientId, _options.Value.ClientSecret),
           new ServiceBusAdministrationClientOptions { });
    }

    public async ValueTask DisposeAsync()
    {
        await _client.DisposeAsync();
    }

    public async Task PublishAsync<TMessage>(MessageEnvelope<TMessage> messageEnvelope, string partitionKey /* todo: on message Envelope */) where TMessage : IMessage
    {
        var serializedMessage = JsonConvert.SerializeObject(messageEnvelope.ToRawMessageEnvelope()); // TODO: Without ToRaw?
        var topic = _topicProvider.TopicFor(messageEnvelope); // i guess, this should be already in envelope or some other DTO object 

        await EnsureTopicExists(topic);
        await using var sender = _client.CreateSender(topic);

        var message = new ServiceBusMessage(serializedMessage)
        {
            PartitionKey = partitionKey,
            SessionId = partitionKey
        };

        await sender.SendMessageAsync(message);
    }

    private async Task EnsureTopicExists(string topic)
    {
        if (!_topics.Contains(topic))
        {
            await _topicsSemaphoreSlim.WaitAsync();
            if (!_topics.Contains(topic))
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
                _topics.Add(topic);
            }
            _topicsSemaphoreSlim.Release();
        }
    }
}
