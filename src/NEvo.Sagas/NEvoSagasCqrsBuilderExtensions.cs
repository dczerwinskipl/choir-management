using Microsoft.Extensions.DependencyInjection;
using NEvo.Core;
using NEvo.Messaging;
using NEvo.Messaging.Events;
using NEvo.Processing.Commands;
using NEvo.Processing.Registering;
using System.Linq.Expressions;

namespace NEvo.Sagas;

public static class NEvoSagasCqrsBuilderExtensions
{
    public static INEvoCqrsExtensionBuilder UseSagas(this INEvoCqrsExtensionBuilder builder)
    {
        builder.UseCustomMessageHandlers();
        return builder;
    }
}

public class SagaHandlerWrapperFactory : IMessageHandlerWrapperFactory
{
    public static MessageHandlerOptions[] MessageHandlerOptions = new[] { 
        new MessageHandlerOptions(typeof(ISagaEventHandler<,>), new SagaHandlerWrapperFactory())
    };

    /// <summary>
    /// Maybe it should just be a static method?
    /// </summary>
    /// <param name="messageHandlerDescription"></param>
    /// <param name="provider"></param>
    /// <returns></returns>
    public IMessageHandlerWrapper Create(MessageHandlerDescription messageHandlerDescription, IServiceProvider provider)
    {
        var type = typeof(CommandHandlerWrapper<,>).MakeGenericType(messageHandlerDescription.HandlerType, messageHandlerDescription.MessageType);
        var wrapper = Activator.CreateInstance(type, new object[] { messageHandlerDescription, provider });
        return Check.Null(wrapper as IMessageHandlerWrapper);
    }
}

public interface ISaga
{
    Guid Id { get; set; }
    bool IsCompleted { get; set; }
}

public interface ISagaRepository<TSaga> where TSaga : ISaga
{
    IEnumerable<TSaga> GetSagas(Expression<Func<TSaga, bool>> predicate);
    Task SaveAsync(TSaga saga);
}

public interface ISagaHandler<TSagaData> where TSagaData : ISaga, new()
{
    public TSagaData Saga { get; }
    public void Map<TMessage>() where TMessage : IMessage;
    public void Complete();
}

public interface ISagaManager
{
    Task GetSagaAsync(IMessage message);
    Task SaveSagaAsync();
}

public abstract class SagaHandler<TSagaData> : ISagaManager, ISagaHandler<TSagaData> where TSagaData : ISaga, new()
{
    private readonly ISagaRepository<TSagaData> _sagaRepository;
    public TSagaData? Saga { get; set; }

    public SagaHandler(ISagaRepository<TSagaData> sagaRepository) 
    {
        _sagaRepository = sagaRepository;
    }

    public void Complete()
    {
        Saga.IsCompleted = true;
    }

    public void Map<TMessage>() where TMessage : IMessage
    {
        throw new NotImplementedException();
    }

    public Task GetSagaAsync(IMessage message)
    {
        throw new NotImplementedException();
    }

    private object GetPredicate(IMessage message)
    {
        throw new NotImplementedException();
    }

    public Task SaveSagaAsync()
    {
        throw new NotImplementedException();
    }
}

public interface ISagaEventHandler<in TEvent, TSagaData> : ISagaHandler<TSagaData> 
                                                            where TSagaData : ISaga, new()
                                                            where TEvent : Event
{
    Task HandleAsync(TEvent @event);
}


public class SagaHandlerWrapper<THandler, TEvent, TSagaData> : IMessageHandlerWrapper
                                                            where THandler : ISagaEventHandler<TEvent, TSagaData>
                                                            where TSagaData : ISaga, new()
                                                            where TEvent : Event
{
    public MessageHandlerDescription Description { get; }
    private readonly IServiceProvider _serviceProvider;

    public SagaHandlerWrapper(MessageHandlerDescription description, IServiceProvider serviceProvider)
    {
        Description = description;
        _serviceProvider = serviceProvider;
    }

    public async Task<Either<Exception, object?>> Handle(IMessage message)
    {
        return (await Try.OfAsync(async () =>
        {
            using var scope = _serviceProvider.CreateScope();
            var handler = ActivatorUtilities.CreateInstance<THandler>(scope.ServiceProvider);
            await handler.HandleAsync(Check.Null(message as TEvent));
        })).Cast<object?>();
    }
}