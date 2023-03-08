using NEvo.Messaging;
using NEvo.Processing.Commands;
using NEvo.Processing.Events;
using NEvo.Processing.Queries;
using NEvo.Processing.Registering;
using NEvo.Processing;
using NEvo.Messaging.Commands;
using NEvo.Messaging.Events;
using NEvo.Messaging.Queries;
using Microsoft.Extensions.DependencyInjection;

namespace NEvo.Core;


/// <summary>
/// Public interface to configure additional options of CQRS
/// </summary>
public interface INEvoCqrsExtensionBuilder
{
    INEvoCqrsExtensionBuilder UseCustomMessageHandlers(params MessageHandlerOptions[] messageHandlerOptions);
}

public static class NEvoServicesBuilderExtensions
{
    public static NEvoServicesBuilder AddCqrs<TMessageBus>(this NEvoServicesBuilder builder, params MessageHandlerOptions[] messageHandlerOptions) where TMessageBus : class, IMessageBus
    {
        builder.UseExtension(new NEvoCqrsExtensionBuilder<TMessageBus>(messageHandlerOptions));
        return builder;
    }
    public static NEvoServicesBuilder AddCqrs<TMessageBus>(this NEvoServicesBuilder builder, Action<INEvoCqrsExtensionBuilder>? configureCqrs = null) where TMessageBus : class, IMessageBus
    {
        var cqrsBuilder = new NEvoCqrsExtensionBuilder<TMessageBus>();
        configureCqrs?.Invoke(cqrsBuilder);
        builder.UseExtension(cqrsBuilder);        
        return builder;
    }
}

public class NEvoCqrsExtensionBuilder<TMessageBus> : INEvoExtensionConfiguration, INEvoCqrsExtensionBuilder where TMessageBus : class, IMessageBus
{
    private Dictionary<Type, MessageHandlerOptions> _messageHandlerOptions;

    public NEvoCqrsExtensionBuilder() : this(
        CommandHandlerWrapperFactory.MessageHandlerOptions, 
        EventHandlerWrapperFactory.MessageHandlerOptions, 
        QueryHandlerWrapperFactory.MessageHandlerOptions) 
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

    public void ConfigureServices(IServiceCollection services)
    {
        services.AddSingleton<IMessageHandlerRegistry, MessageHandlerRegistry>(sp =>
        {
            var registry = new MessageHandlerRegistry(sp, _messageHandlerOptions.Values.ToArray());
            return registry;
        });

        services.AddSingleton<IMessageProcessor, MessageProcessor>();
        services.AddSingleton<ITopicProvider, NEvoNamespaceTopicProvider>();
        services.AddSingleton<IMessageBus, TMessageBus>();
        services.AddSingleton<ICommandDispatcher>(sp => sp.GetRequiredService<IMessageBus>());
        services.AddSingleton<IEventPublisher>(sp => sp.GetRequiredService<IMessageBus>());
        services.AddSingleton<IQueryDispatcher>(sp => sp.GetRequiredService<IMessageBus>());
    }    
}
