using System.ComponentModel.DataAnnotations;
using ChoirManagement.Accounting.Messages;
using NEvo.Core;
using NEvo.CQRS.Messaging;
using NEvo.CQRS.Processing.Commands;
using NEvo.CQRS.Processing.Events;
using NEvo.CQRS.Processing.Queries;
using NEvo.Monads;
using NEvo.ValueObjects;

namespace ChoirManagement.Accounting.Domain.Handlers;
public class HelloWorldHandler : ICommandHandler<HelloWorldCommand>, IEventHandler<HelloWorldEvent>, IQueryHandler<HelloWorldQuery, string>
{
    public IMessageBus _messageBus;

    public HelloWorldHandler(IMessageBus messageBus)
    {
        _messageBus = messageBus;
    }

    public async Task<Either<Exception, Unit>> HandleAsync(HelloWorldCommand command)
    {
        await _messageBus.PublishAsync(new HelloWorldEvent(command.Message, ObjectId.New(nameof(HelloWorldCommand), command.Message)));

        if (command.Message.Equals("Hi", StringComparison.InvariantCultureIgnoreCase))
            return Either.Failure(new ValidationException("Don't say hi!"));

        return Either.Success();
    }

    public async Task HandleAsync(HelloWorldEvent @event)
    {
        Console.WriteLine($"Start");
        await Task.Delay(2000);
        Console.WriteLine($"{@event.Message}");
        Console.WriteLine($"Stop");
    }

    public async Task<string> HandleAsync(HelloWorldQuery query)
    {
        return "Hello my friendo!";
    }
}
