using NEvo.Messaging;
using NEvo.Processing.Registering;

namespace NEvo.Core.Processing;

public class MessageProcessor : IMessageProcessor
{
    private IMessageHandlerRegistry _messageHandlerRegistry;
    public MessageProcessor(IMessageHandlerRegistry messageHandlerRegistry)
    {
        _messageHandlerRegistry = Check.Null(messageHandlerRegistry);
    }

    public async Task<Either<IMessageProcessingFailures, TResult>> ProcessAsync<TResult>(IMessage<TResult> message)
    {
        var handlerDescriptions = _messageHandlerRegistry.GetHandlers(message);
        //todo: pipeline
        var handlerWrapper = handlerDescriptions.Single();

        try
        {
            return (await handlerWrapper.Handle(message))
                            .Handle(
                                success => Either.Right<IMessageProcessingFailures, TResult>(success.Result),
                                error => Either.Left<IMessageProcessingFailures, TResult>(new MessageProcessingFailures(error))
                                );
        }
        catch (Exception exc)
        {
            return Either.Left<IMessageProcessingFailures, TResult>(
                new MessageProcessingFailures(new MessageProcessingFailure(handlerWrapper.Description.HandlerType, exc))
            );
        }
    }

    public async Task<Either<IMessageProcessingFailures, Void>> ProcessAsync(IMessage message)
    {
        var handlerWrappers = _messageHandlerRegistry.GetHandlers(message);
        var errors = new List<MessageProcessingFailure>();
        var result = new List<MessageProcessingSuccess>();

        //todo: pipeline
        foreach (var handlerWrapper in handlerWrappers)
        {
            try
            {
                (await handlerWrapper.Handle(message)).Handle(result.Add, errors.Add);
            }
            catch (Exception exc)
            {
                errors.Add(new MessageProcessingFailure(handlerWrapper.Description.HandlerType, exc));
            }
        }

        return errors.Any() ?
            Either.Left<IMessageProcessingFailures, Void>(new MessageProcessingFailures(errors)) :
            Either.Right<IMessageProcessingFailures, Void>(Void.Instance);
    }
}
