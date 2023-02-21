using NEvo.Core;
using NEvo.Messaging;

namespace NEvo.Processing;

public interface IMessageProcessor
{
    public Task<Either<IMessageProcessingFailures, TResult>> ProcessAsync<TMessage, TResult>(TMessage message) where TMessage: IMessage<TResult>;
}
