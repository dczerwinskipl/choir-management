using NEvo.Monads;
using NEvo.Messaging;

namespace NEvo.Processing.Registering;

/// <summary>
/// Message handler wrapper interface to unify message handling with result and allow to add custom type of handlers as extension
/// </summary>
/// <typeparam name="TResult"></typeparam>
public interface IMessageHandlerAdapter
{
    MessageHandlerDescription Description { get; }
    Task<Either<Exception, object?>> HandleAsync(IMessage message);
}

public interface IMessageHandlerWrapper<TResult> : IMessageHandlerAdapter
{
    Task<Either<Exception, TResult?>> Handle(IMessage<TResult> message);
}

public class GenericMessageHandlerWrapper<TResult> : IMessageHandlerWrapper<TResult>
{
    private readonly IMessageHandlerAdapter _originalWrapper;
    public MessageHandlerDescription Description => _originalWrapper.Description;

    internal GenericMessageHandlerWrapper(IMessageHandlerAdapter originalWrapper) => _originalWrapper = originalWrapper;
    public async Task<Either<Exception, TResult?>> Handle(IMessage<TResult> message) => 
        await _originalWrapper.HandleAsync(message).Map(
            success => Either.Success((TResult?)success),
            Either.Failure<TResult?>
        );

    public Task<Either<Exception, object?>> HandleAsync(IMessage message) => _originalWrapper.HandleAsync(message);
}

public static class MessageHandlerWrapperExtensions
{
    public static IMessageHandlerWrapper<TResult> ToGeneric<TResult>(this IMessageHandlerAdapter handler) => new GenericMessageHandlerWrapper<TResult>(handler);
}
