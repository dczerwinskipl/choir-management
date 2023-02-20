using NEvo.Messaging;

namespace NEvo.Core.Processing;

public interface IMessageProcessor
{
    public Task<Either<IMessageProcessingFailures, Void>> ProcessAsync(IMessage message);
    public Task<Either<IMessageProcessingFailures, TResult>> ProcessAsync<TResult>(IMessage<TResult> message);
}
