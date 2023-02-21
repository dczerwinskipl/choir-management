using NEvo.Core;
using NEvo.Messaging;

namespace NEvo.Processing.Registering;

/// <summary>
/// Message handler wrapper interface to unify message handling with result and allow to add custom type of handlers as extension
/// </summary>
/// <typeparam name="TResult"></typeparam>
public interface IMessageHandlerWrapper
{
    MessageHandlerDescription Description { get; }
    Task<Try<object>> Handle(IMessage message);
}

public interface IMessageHandlerWrapper<TResult> : IMessageHandlerWrapper
{
    Task<Try<TResult>> Handle(IMessage<TResult> message);
}

public class GenericMessageHandlerWrapper<TResult> : IMessageHandlerWrapper<TResult>
{
    private readonly IMessageHandlerWrapper _originalWrapper;
    public MessageHandlerDescription Description => _originalWrapper.Description;

    internal GenericMessageHandlerWrapper(IMessageHandlerWrapper originalWrapper) => _originalWrapper = originalWrapper;
    public async Task<Try<TResult>> Handle(IMessage<TResult> message) =>
        (await _originalWrapper.Handle(message)).Handle(
            success => Try.Success((TResult)success),
            Try.Failure<TResult>
            );

    public Task<Try<object>> Handle(IMessage message) => _originalWrapper.Handle(message);
}

public static class MessageHandlerWrapperExtensions
{
    public static IMessageHandlerWrapper<TResult> ToGeneric<TResult>(this IMessageHandlerWrapper handler) => new GenericMessageHandlerWrapper<TResult>(handler);
}
