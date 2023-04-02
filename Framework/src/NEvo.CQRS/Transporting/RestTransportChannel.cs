using Microsoft.Extensions.Options;
using NEvo.Core;
using NEvo.CQRS.Messaging;
using NEvo.Monads;

namespace NEvo.CQRS.Transporting;

public class RestTransportChannel : ITransportChannel
{
    public RestTransportChannel()
    {

    }

    public Task<Either<Exception, TResult>> DispatchMessageAsync<TMessage, TResult>(MessageEnvelope<TMessage> messsageEnvelope) where TMessage : IMessage<TResult>
    {
        throw new NotImplementedException();
    }

    public Task<Either<Exception, Unit>> PublishMessageAsync<TMessage>(MessageEnvelope<TMessage> messsageEnvelope) where TMessage : IMessage<Unit>
    {
        throw new NotImplementedException();
    }
}
