using Azure.Messaging.ServiceBus;
using NEvo.CQRS.Messaging;
using NEvo.CQRS.Messaging.Events;
using NEvo.CQRS.Processing;
using NEvo.Polling;
using Newtonsoft.Json;

namespace NEvo.Azure.Polling;

public class AzureServiceBusMessagePoller : IMessagePoller, IAsyncDisposable
{
    private readonly IMessageProcessor _messageProcessor;
    private readonly string _topicName;
    private readonly string _subscriptionName;
    private readonly ServiceBusClient _client;

    public Guid Id { get; } = Guid.NewGuid();
    private ServiceBusSessionProcessor _processor;

    public AzureServiceBusMessagePoller(ServiceBusClient client, IMessageProcessor messageProcessor, string topicName, string subscriptionName)
    {
        _messageProcessor = messageProcessor;
        _topicName = topicName;
        _subscriptionName = subscriptionName;
        _client = client;
        _processor = CreateProcessor();
    }

    public ServiceBusSessionProcessor CreateProcessor()
    {
        var processor = _client.CreateSessionProcessor(_topicName, _subscriptionName, new ServiceBusSessionProcessorOptions { AutoCompleteMessages = false });
        processor.ProcessMessageAsync += async (@event) => { await ProcessMessage(Id, @event.Message); await @event.CompleteMessageAsync(@event.Message); };
        processor.ProcessErrorAsync += Processor_ProcessErrorAsync;
        return processor;
    }

    private Task Processor_ProcessErrorAsync(ProcessErrorEventArgs arg)
    {
        //how it works?
        return Task.CompletedTask;
    }

    private async Task ProcessMessage(Guid id, ServiceBusReceivedMessage serviceBusMessage)
    {
        if (serviceBusMessage is null)
            return;

        Console.WriteLine($"[{id}] - got message - {serviceBusMessage}");

        Event message;
        try
        {
            var envelope = JsonConvert.DeserializeObject<MessageEnvelope>(serviceBusMessage.Body.ToString());
            message = (Event)JsonConvert.DeserializeObject(envelope.Payload, Type.GetType(envelope.MessageType));
        }
        catch (Exception)
        {
            //todo: log error
            return;
        }

        if (message is null)
            return;

        //TODO: everything here should be pushed away from Azure, ex. DLQ, etc.
        try
        {
            await _messageProcessor.ProcessAsync(message);
        }
        catch (Exception)
        {
            //todo: move to DLQ or sth
        }
    }

    public async ValueTask DisposeAsync()
    {
        if (_processor is not null)
        {
            await _processor.DisposeAsync();
            _processor = null;
        }

        GC.SuppressFinalize(this);
    }

    public async Task StartAsync(CancellationToken cancellationToken)
    {
        await _processor.StartProcessingAsync(cancellationToken);
    }

    public async Task StopAsync()
    {
        await _processor.StopProcessingAsync();
    }
}
