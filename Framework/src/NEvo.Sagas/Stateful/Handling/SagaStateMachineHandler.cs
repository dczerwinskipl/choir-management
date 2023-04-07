using System.Collections.Concurrent;
using System.Linq.Expressions;
using NEvo.Core;
using NEvo.Core.StateManaging;
using NEvo.CQRS.Messaging;
using NEvo.Monads;
using NEvo.Sagas.Stateful.Building;

namespace NEvo.Sagas.Stateful.Handling;

public delegate Expression<Func<TSaga, bool>> SagaFinderDelegate<TSaga>(IMessage message);

public interface ISagaFinder<TSaga>
{
    Type MessageType { get; }

    Expression<Func<TSaga, bool>> FindPredicate(IMessage message);
}

public class SagaFinder<TSaga, TMessage> : ISagaFinder<TSaga> where TMessage : IMessage
{
    private readonly SagaFinderDelegate<TSaga> _finder;

    public Type MessageType => typeof(TMessage);

    public SagaFinder(SagaFinderDelegate<TSaga> finder)
    {
        _finder = finder;
    }

    public Expression<Func<TSaga, bool>> FindPredicate(IMessage message) => _finder(message);
}


public interface ISagaStateMachineHandler
{
    public void Init();
    public IEnumerable<Type> GetMessageTypes();
}

public interface ISagaStateMachineHandler<TSaga, TState> : ISagaStateMachineHandler where TSaga : IStatefulSaga<TState>
{
    Expression<Func<TSaga, bool>> FindSagaPredicate(IMessage message);
    Task<Either<Exception, Maybe<TSaga>>> HandleAsync<TMessage>(TMessage message, Maybe<TSaga> sagaContext) where TMessage : IMessage;
}


public record NoTransitionForMessage(IMessage Message, string Reason) : Refused(Reason);

public abstract class SagaStateMachineHandler<TSaga, TState> : ISagaStateMachineHandler<TSaga, TState> where TSaga : IStatefulSaga<TState>
{
    private SagaStateMachineDefinition<TSaga, TState>? _definition;
    private readonly SagaStateMachineBuilder<TSaga, TState> _builder;
    private readonly ConcurrentDictionary<Type, ISagaFinder<TSaga>?> _messageListeners = new ConcurrentDictionary<Type, ISagaFinder<TSaga>?>();
    public static readonly string InitStateName = "InitState";

    public bool IgnoreNotDefinedTransitions = false;

    public SagaStateMachineHandler()
    {
        _builder = new SagaStateMachineBuilder<TSaga, TState>();
        _builder.AddInitState(InitStateName);
    }

    public async Task<Either<Exception, Maybe<TSaga>>> HandleAsync<TMessage>(TMessage message, Maybe<TSaga> sagaContext) where TMessage : IMessage
        => await sagaContext
                    .MatchAsync(
                        async (context) => await _definition!.LoadSagaStateMachineAsync(context),
                        async () => await _definition!.NewSagaStateMachineAsync()
                    )
                    .BindAsync(
                        async stateMachine => await ApplyMessageAsync(stateMachine, message)
                    );

    private async Task<Either<Exception, Maybe<TSaga>>> ApplyMessageAsync<TMessage>(StateMachine<TSaga> stateMachine, TMessage message) where TMessage : IMessage
        => await _definition!
                    .GetMessageTransition<TMessage>(stateMachine.CurrentState)
                    .MatchAsync(
                        async transition => await stateMachine.ApplyTransitionAsync(transition, message),
                        () => new NoTransitionForMessage(message, $"Transition not defined")
                    )
                    /*its only for saga???, TODO: change API of monads*/
                    .Then(success => _definition.SaveStateOnContext(stateMachine), failure => { })
                    .Map(
                        success => Either.Right<Exception, Maybe<TSaga>>(Maybe.Some(stateMachine.Context)),
                        refused => refused switch
                                    {
                                        NoTransitionForMessage => Either.Right<Exception, Maybe<TSaga>>(Maybe.None),
                                        Refused.TransitionNotFound => Either.Right<Exception, Maybe<TSaga>>(Maybe.None),
                                        _ => new TransitionRefusedException(refused)
                                    }
                    );

    protected void Saga<T>() where T : TSaga, new()
    {
        _builder.UseEmptySaga<T>();
    }
    protected void Saga(TSaga emptySaga)
    {
        _builder.UseEmptySaga(emptySaga);
    }


    protected void Initialy(params SagaStateMachineHandlerTransitionConfiguration<TSaga>[] triggers)
    {
        _builder.ConfigureState(InitStateName, stateBuilder => triggers.ForEach(trigger => trigger.Apply(stateBuilder)));
    }

    protected void State(string stateName, params SagaStateMachineHandlerTransitionConfiguration<TSaga>[] triggers)
    {
        _builder.AddState(stateName, stateBuilder => triggers.ForEach(trigger => trigger.Apply(stateBuilder)));
    }

    protected void AnyState(params SagaStateMachineHandlerTransitionConfiguration<TSaga>[] triggers)
    {
        //_builder.AddTransition(stateBuilder => triggers.ForEach(trigger => trigger.Apply(stateBuilder));
    }

    public void FindSaga(params ISagaFinder<TSaga>[] finders)
    {
        finders.ForEach(finder => _messageListeners.AddOrUpdate(finder.MessageType, _ => finder, (_, __) => finder));
    }

    public SagaFinder<TSaga, TMessage> ByMessage<TMessage>(Func<TMessage, Expression<Func<TSaga, bool>>> predicate) where TMessage : IMessage => new SagaFinder<TSaga, TMessage>(message => predicate((TMessage)message));

    public SagaStateMachineHandlerMessageConfiguration<TSaga, TMessage> OnMessage<TMessage>() where TMessage : IMessage
    {
        var eventConfiguration = new SagaStateMachineHandlerMessageConfiguration<TSaga, TMessage>();
        _messageListeners.GetOrAdd(typeof(TMessage), (ISagaFinder<TSaga>?)null);
        return eventConfiguration;
    }
    public SagaStateMachineHandlerTimeoutConfiguration<TSaga> OnTimer(TimeSpan timeout)
    {
        var timeoutConfiguration = new SagaStateMachineHandlerTimeoutConfiguration<TSaga>(timeout);
        // add timeout to schedule?
        return timeoutConfiguration;
    }

    public void Init()
    {
        _definition = _builder.Build();
    }

    public Expression<Func<TSaga, bool>> FindSagaPredicate(IMessage message) => _messageListeners[message.GetType()].FindPredicate(message);

    public IEnumerable<Type> GetMessageTypes() => _messageListeners.Keys;
}

public interface ISagaRrepository<TSaga, TState> where TSaga : IStatefulSaga<TState>
{
    Task<Maybe<TSaga>> GetAsync(Expression<Func<TSaga, bool>> predicate);
    Task SaveAsync(TSaga newSagaContext);
}