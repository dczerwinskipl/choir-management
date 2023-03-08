using ChoirManagement.Accounting.Messages;
using NEvo.Core;
using NEvo.Messaging;
using NEvo.Processing.Commands;
using NEvo.Processing.Events;
using NEvo.Processing.Queries;
using NEvo.ValueObjects;
using System.ComponentModel.DataAnnotations;

namespace ChoirManagement.Accounting.Domain.Handlers;
public class HelloWorldHandler : ICommandHandler<HelloWorldCommand>, IEventHandler<HelloWorldEvent>, IQueryHandler<HelloWorldQuery, string>
{
    public IMessageBus _messageBus;

    public HelloWorldHandler(IMessageBus messageBus)
    {
        _messageBus = messageBus;
    }

    public async Task<Try<Unit>> HandleAsync(HelloWorldCommand command)
    {
        await _messageBus.PublishAsync(new HelloWorldEvent(command.Message, SourceId.New(nameof(HelloWorldCommand), command.Message)));
        
        if (command.Message.Equals("Hi", StringComparison.InvariantCultureIgnoreCase))
            return Try.Failure(new ValidationException("Don't say hi!"));

        return Try.Success();
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
