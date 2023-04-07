using System.Collections.Concurrent;
using System.Text.RegularExpressions;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using NEvo.CQRS.Messaging;
using NEvo.CQRS.Messaging.Commands;
using NEvo.CQRS.Messaging.Events;
using NEvo.CQRS.Messaging.Queries;
using NEvo.CQRS.Processing;
using NEvo.CQRS.Processing.Commands;
using NEvo.CQRS.Processing.Events;
using NEvo.CQRS.Processing.Queries;
using NEvo.CQRS.Processing.Registering;
using NEvo.CQRS.Routing;
using NEvo.CQRS.Transporting;
using static NEvo.CQRS.Transporting.TransportChannelDescription;

namespace NEvo.Core;

public class NEvoCqrsExtensionBuilder : INEvoExtensionConfiguration, INEvoCqrsExtensionBuilder
{
    private readonly Dictionary<Type, MessageHandlerOptions> _messageHandlerOptions;
    private readonly List<Action<RoutingTopologyDescription>> _routingTopologyDescriptionConfigurations = new();
    private readonly List<Action<RoutingPolicyDescription>> _routingPolicyDescriptionConfigurations = new();

    public NEvoCqrsExtensionBuilder() : this(
        CommandHandlerAdapterFactory.MessageHandlerOptions,
        EventHandlerAdapterFactory.MessageHandlerOptions,
        QueryHandlerAdapterFactory.MessageHandlerOptions)
    {
    }

    public NEvoCqrsExtensionBuilder(params MessageHandlerOptions[] messageHandlerOptions)
    {
        Check.Null(messageHandlerOptions);
        _messageHandlerOptions = messageHandlerOptions.ToDictionary(mho => mho.HandlerInterface);
    }

    public INEvoCqrsExtensionBuilder UseCustomMessageHandlers(params MessageHandlerOptions[] messageHandlerOptions)
    {
        Check.Null(messageHandlerOptions);
        foreach (var element in messageHandlerOptions)
        {
            _messageHandlerOptions.TryAdd(element.HandlerInterface, element);
        }
        return this;
    }
    public INEvoCqrsExtensionBuilder ConfigureRoutingTopology(Action<RoutingTopologyDescription> topologyConfiguration)
    {
        _routingTopologyDescriptionConfigurations.Add(topologyConfiguration);
        return this;
    }

    public INEvoCqrsExtensionBuilder ConfigureRoutingPolicy(Action<RoutingPolicyDescription> policyConfiguration)
    {
        _routingPolicyDescriptionConfigurations.Add(policyConfiguration);
        return this;
    }

    public void ConfigureServices(IServiceCollection services)
    {
        services.AddSingleton<IMessageHandlerRegistry, MessageHandlerRegistry>(sp =>
        {
            var registry = new MessageHandlerRegistry(sp, _messageHandlerOptions.Values.ToArray());
            return registry;
        });

        services.AddSingleton<IMessageProcessor, MessageProcessor>();
        services.AddScoped<IMessageBus, MessageBus>();
        services.AddScoped<IMessageRouter, MessageRouter>();
        services.Configure<TransportChannelFactoryOptions>(options =>
        {
            options.InternalTransportChannelFactory = new InternalTransportChannelFactory();
        });
        services.AddSingleton<ITransportChannelFactory, TransportChannelFactory>();

        foreach (var configuration in _routingTopologyDescriptionConfigurations)
        {
            services.Configure(configuration);
        }
        services.AddSingleton<IRoutingTopologyProvider, RoutingTopologyProvider>();

        foreach (var configuration in _routingPolicyDescriptionConfigurations)
        {
            services.Configure(configuration);
        }
        services.AddSingleton<IRoutingPolicyFactory, RoutingPolicyFactory>();

        services.AddScoped<IMessageBus, MessageBus>();
        services.AddScoped<ICommandDispatcher>(sp => sp.GetRequiredService<IMessageBus>());
        services.AddScoped<IEventPublisher>(sp => sp.GetRequiredService<IMessageBus>());
        services.AddScoped<IQueryDispatcher>(sp => sp.GetRequiredService<IMessageBus>());
    }

    public INEvoCqrsExtensionBuilder UseConfiguration(IConfiguration configuration)
    {
        throw new NotImplementedException();
    }
}


public class RoutingPolicyFactory : IRoutingPolicyFactory
{
    private readonly ConcurrentDictionary<Type, IRoutingPolicy> _policies = new();
    private readonly List<RoutingPolicyRule> _rules;

    public RoutingPolicyFactory(IOptions<RoutingPolicyDescription> options)
    {
        _rules = Check.Null(options.Value.Rules);
    }

    public IRoutingPolicy CreatePolicyFor<TMessage>(TMessage message) where TMessage : IMessage
    {
        return _policies.GetOrAdd(message.GetType(), CreatePolicyForInternal<TMessage>);
    }

    private IRoutingPolicy CreatePolicyForInternal<TMessage>(Type message) where TMessage : IMessage
    {
        if (TMessage.MessageType == MessageType.Event)
        {
            return new RoutingPolicy(TMessage.MessageType, GetChannel(message));
        }
        return new RoutingPolicy(TMessage.MessageType);
    }

    private string? GetChannel(Type message)
    {
        var @namespace = message.Namespace;
        foreach (var rule in _rules)
        {
            if (Regex.IsMatch(@namespace, rule.NamespacePattern))
                return rule.ChannelName;
        }
        return null;
    }
}
public class RoutingPolicy : IRoutingPolicy
{
    public readonly MessageType _messageType;
    private readonly string? _channelName;

    public RoutingPolicy(MessageType messageType, string? channelName = null)
    {
        _messageType = messageType;
        _channelName = channelName;
    }

    public TransportChannelDescription GetChannelDescription()
    {
        return _messageType switch
        {
            MessageType.Command => new InternalTransportChannelDescription(false),
            MessageType.Query => new InternalTransportChannelDescription(false),
            MessageType.Event => _channelName is null ? new InternalTransportChannelDescription(false) : new MessagePublisherTransportChannelDescription(_channelName)
        };
    }
}


public class RoutingPolicyDescription
{
    public List<RoutingPolicyRule> Rules { get; set; } = new();
}

public class RoutingPolicyRule
{
    public string NamespacePattern { get; set; }
    public string ChannelName { get; set; }
}