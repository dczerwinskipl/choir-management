using NEvo.Core;
using NEvo.Messaging;
using NEvo.Processing.Registering;

namespace NEvo.Processing;

public class MessageProcessor : IMessageProcessor
{
    private IMessageHandlerRegistry _messageHandlerRegistry;
    public MessageProcessor(IMessageHandlerRegistry messageHandlerRegistry)
    {
        _messageHandlerRegistry = Check.Null(messageHandlerRegistry);
    }

    public async Task<Either<IMessageProcessingFailures, TResult>> ProcessAsync<TMessage, TResult>(TMessage message) where TMessage : IMessage<TResult>
    {
        var handlerDescriptions = _messageHandlerRegistry.GetHandlers<TMessage, TResult>(message);
        //todo: pipeline
        var handlerWrapper = handlerDescriptions.Single();

        try
        {
            return (await handlerWrapper.Handle(message))
                            .Handle(
                                Either.Right<IMessageProcessingFailures, TResult>,
                                error => Either.Left<IMessageProcessingFailures, TResult>(new MessageProcessingFailures(new MessageProcessingFailure(handlerWrapper.Description.HandlerType, error)))
                                );
        }
        catch (Exception exc)
        {
            return Either.Left<IMessageProcessingFailures, TResult>(
                new MessageProcessingFailures(new MessageProcessingFailure(handlerWrapper.Description.HandlerType, exc))
            );
        }
    }

    public async Task<Either<IMessageProcessingFailures, Unit>> ProcessAsync<TMessage>(TMessage message) where TMessage : IMessage<Unit>
    {
        var handlerWrappers = _messageHandlerRegistry.GetHandlers<TMessage, Unit>(message);
        var errors = new List<MessageProcessingFailure>();
        var result = new List<Unit>();

        //todo: pipeline
        foreach (var handlerWrapper in handlerWrappers)
        {
            try
            {
                (await handlerWrapper.Handle(message)).OnFailure(exc => errors.Add(new MessageProcessingFailure(handlerWrapper.Description.HandlerType, exc)));
            }
            catch (Exception exc)
            {
                errors.Add(new MessageProcessingFailure(handlerWrapper.Description.HandlerType, exc));
            }
        }

        return errors.Any() ?
            Either.Left<IMessageProcessingFailures, Unit>(new MessageProcessingFailures(errors)) :
            Either.Right<IMessageProcessingFailures, Unit>(Unit.Value);
    }
}
