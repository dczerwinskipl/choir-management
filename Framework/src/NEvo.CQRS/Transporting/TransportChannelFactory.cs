using Microsoft.Extensions.Options;
using NEvo.Core;
using NEvo.Core.Reflection;
using NEvo.CQRS.Routing;
using static NEvo.CQRS.Transporting.TransportChannelDescription;

namespace NEvo.CQRS.Transporting;

public class TransportChannelFactory : ITransportChannelFactory
{
    private readonly ITypeResolver _typeResolver;
    private readonly ITransportChannelFactory<ITransportChannel, InternalTransportChannelDescription, InternalChannelTopologyDescription> _internalTransportChannelFactory;
    private readonly Dictionary<Type, ITransportChannelFactory<ITransportChannel, EndpointTransportChannelDescription, EndpointTopologyDescription>> _endpointTransportChannelFactories = new();
    private readonly Dictionary<Type, ITransportChannelFactory<ITransportChannel, MessagePublisherTransportChannelDescription, TopicTopologyDescription>> _messagePublisherTransportChannelFactories = new();
    private readonly IRoutingTopologyProvider _routingTopologyProvider;

    public TransportChannelFactory(ITypeResolver typeResolver, IRoutingTopologyProvider routingTopologyProvider, IOptions<TransportChannelFactoryOptions> options)
    {
        _typeResolver = Check.Null(typeResolver);
        _routingTopologyProvider = Check.Null(routingTopologyProvider);
        _internalTransportChannelFactory = Check.Null(options?.Value?.InternalTransportChannelFactory);
        AddToDictionary(Check.Null(options?.Value?.EndpointTransportChannelFactories));
        AddToDictionary(Check.Null(options?.Value?.MessagePublisherTransportChannelFactories));
    }

    private void AddToDictionary(List<ITransportChannelFactory<ITransportChannel, EndpointTransportChannelDescription, EndpointTopologyDescription>> factories)
    {
        foreach(var factory in factories)
        {
            foreach(var type in GetTransportChannelType<EndpointTransportChannelDescription>(factory))
            {
                _endpointTransportChannelFactories.Add(type, factory);
            }
        }
    }

    private void AddToDictionary(List<ITransportChannelFactory<ITransportChannel, MessagePublisherTransportChannelDescription, TopicTopologyDescription>> factories)
    {
        foreach (var factory in factories)
        {
            foreach (var type in GetTransportChannelType<MessagePublisherTransportChannelDescription>(factory))
            {
                _messagePublisherTransportChannelFactories.Add(type, factory);
            }
        }
    }

    private IEnumerable<Type> GetTransportChannelType<TDescription>(object factory) where TDescription : TransportChannelDescription =>
        factory
            .GetType()
            .GetInterfaces()
            .Where(i => i.IsGenericType && i.GetGenericTypeDefinition() == typeof(ITransportChannelFactory<,,>) && i.GetGenericArguments()[1].IsAssignableFrom(typeof(TDescription)))
            .Select(i => i.GetGenericArguments()[0]);

    public ITransportChannel CreateChannel(IServiceProvider serviceProvider, TransportChannelDescription channelDescription) => 
        channelDescription switch
        {
            InternalTransportChannelDescription internalChannelDescription => CreateTransportChannel(serviceProvider, internalChannelDescription),
            EndpointTransportChannelDescription endpointChannelDescription => CreateTransportChannel(serviceProvider, endpointChannelDescription),
            MessagePublisherTransportChannelDescription messagePublisherChannelDescription => CreateTransportChannel(serviceProvider, messagePublisherChannelDescription),
            _ => throw new NotImplementedException()
        };

    private ITransportChannel CreateTransportChannel(IServiceProvider serviceProvider, InternalTransportChannelDescription internalChannelDescription)
    {
        return _internalTransportChannelFactory.CreateChannel(serviceProvider, internalChannelDescription, null);
    }

    private ITransportChannel CreateTransportChannel(IServiceProvider serviceProvider, EndpointTransportChannelDescription endpointChannelDescription)
    {
        var endpointDescription = _routingTopologyProvider.GetEndpointDescription(endpointChannelDescription.EndpointName);
        var type = _typeResolver.GetType(endpointDescription.ChannelType);
        return _endpointTransportChannelFactories.TryGetValue(type, out var factory) ? factory.CreateChannel(serviceProvider, endpointChannelDescription, endpointDescription) : throw new ArgumentException($"No channel factory found for Endpoint {endpointChannelDescription.EndpointName} of type {endpointDescription.ChannelType}");
    }

    private ITransportChannel CreateTransportChannel(IServiceProvider serviceProvider, MessagePublisherTransportChannelDescription messagePublisherChannelDescription)
    {
        var topicDescription = _routingTopologyProvider.GetTopicDescription(messagePublisherChannelDescription.TopicName);
        var type = _typeResolver.GetType(topicDescription.ChannelType);
        return _messagePublisherTransportChannelFactories.TryGetValue(type, out var factory) ? factory.CreateChannel(serviceProvider, messagePublisherChannelDescription, topicDescription) : throw new ArgumentException($"No channel factory found for Topic {messagePublisherChannelDescription.TopicName} of type {topicDescription.ChannelType}");
    }
}