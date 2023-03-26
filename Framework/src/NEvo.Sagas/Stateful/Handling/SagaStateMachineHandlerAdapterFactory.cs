using NEvo.Core;
using NEvo.Processing.Registering;
using System.Collections.Concurrent;

namespace NEvo.Sagas.Stateful.Handling;

public class SagaStateMachineHandlerAdapterFactory : IMessageHandlerAdapterFactory
{
    public static MessageHandlerOptions MessageHandlerOptions = new MessageHandlerOptions(typeof(ISagaStateMachineHandler<,>), new SagaStateMachineHandlerAdapterFactory());

    private ConcurrentDictionary<Type, ISagaStateMachineHandler> _sagaHandlers = new ConcurrentDictionary<Type, ISagaStateMachineHandler>();

    public IMessageHandlerAdapter Create(MessageHandlerDescription messageHandlerDescription, IServiceProvider provider)
    {
        var interfaceGenericTypes = messageHandlerDescription.InterfaceType.GetGenericArguments();
        var type = typeof(SagaStateMachineHandlerAdapter<,,,>).MakeGenericType(
            messageHandlerDescription.HandlerType, 
            messageHandlerDescription.MessageType,
            interfaceGenericTypes[0],
            interfaceGenericTypes[1]
            );
        var wrapper = Activator.CreateInstance(type, new object[] { messageHandlerDescription, _sagaHandlers[messageHandlerDescription.HandlerType], provider });
        return Check.Null(wrapper as IMessageHandlerAdapter);
    }

    public IEnumerable<MessageHandlerDescription> GetMessageHandlerDescriptions(Type handlerType, Type handlerInterface)
    {
        var stateMachineSagaHandler = _sagaHandlers.GetOrAdd(handlerType, (key) => CreateSagaStateMachineHandler(handlerType));
        foreach (var messageType in stateMachineSagaHandler.GetMessageTypes())
        {
            yield return new MessageHandlerDescription(
                    handlerType,
                    messageType,
                    handlerInterface,
                    handlerType.GetInterfaceMap(handlerInterface)
                                .TargetMethods
                                .Single(t => t.Name == nameof(ISagaStateMachineHandler<IStatefulSaga<object>, object>.HandleAsync)));
        }
    }

    private static ISagaStateMachineHandler CreateSagaStateMachineHandler(Type handlerType)
    {
        var sagaStateMachine = Activator.CreateInstance(handlerType) as ISagaStateMachineHandler;
        sagaStateMachine.Init();
        return sagaStateMachine;
    }
}
