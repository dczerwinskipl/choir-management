using NEvo.Core;
using NEvo.CQRS.Messaging;
using NEvo.Monads;

namespace NEvo.CQRS.Transporting;

public interface ITransportChannel
{
    Task<Either<Exception, TResult>> DispatchMessageAsync<TMessage, TResult>(MessageEnvelope<TMessage> messageEnvelope) where TMessage : IMessage<TResult>;
    Task<Either<Exception, Unit>> PublishMessageAsync<TMessage>(MessageEnvelope<TMessage> messageEnvelope) where TMessage : IMessage<Unit>;
}
