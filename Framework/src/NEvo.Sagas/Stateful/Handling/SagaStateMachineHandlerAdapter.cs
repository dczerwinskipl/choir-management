using Microsoft.Extensions.DependencyInjection;
using NEvo.Core;
using NEvo.Messaging;
using NEvo.Monads;
using NEvo.Processing.Registering;

namespace NEvo.Sagas.Stateful.Handling;

public class SagaStateMachineHandlerAdapter<TSagaHandler, TMessage, TSaga, TState> : IMessageHandlerAdapter
                                                                                        where TMessage : IMessage
                                                                                        where TSagaHandler : ISagaStateMachineHandler<TSaga, TState>
                                                                                        where TSaga : IStatefulSaga<TState>
{
    public MessageHandlerDescription Description { get; init; }
    private ISagaStateMachineHandler<TSaga, TState> _sagaHandler;
    private IServiceProvider _serviceProvider;

    public SagaStateMachineHandlerAdapter(MessageHandlerDescription description, ISagaStateMachineHandler<TSaga, TState> sagaHandler, IServiceProvider serviceProvider)
    {
        Description = description;
        _sagaHandler = sagaHandler;
        _serviceProvider = serviceProvider;
    }

    public async Task<Either<Exception, Unit>> HandleAsync2(IMessage message)
    {
        return await Try.OfAsync(async () =>
        {
            using var scope = _serviceProvider.CreateScope();

            /* maybe that logic should go to other class and keep wrapper only as wrapper */
            var sagaRepository = scope.ServiceProvider.GetRequiredService<ISagaRrepository<TSaga, TState>>();
            var predicate = _sagaHandler.FindSagaPredicate(message);
            var sagaContext = await sagaRepository.GetAsync(predicate);
            var newSagaContext = 
                    await _sagaHandler
                            .HandleAsync((TMessage)message, sagaContext)
                            .ThenAsync(
                                // tu coś poszło nie tak
                                saga => saga.MatchAsync(async some => { await sagaRepository.SaveAsync(some); return Unit.Value; }, async () => { return Unit.Value; })
                            );
        });
    }

    public async Task<Either<Exception, object?>> HandleAsync(IMessage message) => (await HandleAsync2(message)).Cast<object?>();
}