using NEvo.Core;
using NEvo.CQRS.Messaging;
using NEvo.Monads;

namespace NEvo.CQRS.Processing;

public interface IMessageProcessor
{
    public Task<Either<IMessageProcessingFailures, TResult>> ProcessAsync<TMessage, TResult>(TMessage message) where TMessage : IMessage<TResult>;
    public Task<Either<IMessageProcessingFailures, Unit>> ProcessAsync<TMessage>(TMessage message) where TMessage : IMessage<Unit>;
}
