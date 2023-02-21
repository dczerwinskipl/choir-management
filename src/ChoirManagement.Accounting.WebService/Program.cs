using Microsoft.AspNetCore.Mvc;
using NEvo.Core;
using NEvo.Messaging;
using NEvo.Messaging.Commands;
using NEvo.Messaging.Events;
using NEvo.Messaging.Queries;
using NEvo.Processing;
using NEvo.Processing.Commands;
using NEvo.Processing.Events;
using NEvo.Processing.Queries;
using NEvo.Processing.Registering;
using NEvo.ValueObjects;
using System.Globalization;

var instanceId = Guid.NewGuid();
var builder = WebApplication.CreateBuilder(args);
UseNEvo(builder.Services);

var app = builder.Build();


app.MapGet("/", () => $"Hello World from instance {instanceId}");

app.MapGet("/HelloWorld/{message}", async ([FromServices] IMessageBus messageBus, [FromRoute] string message) => ToResponse(
    await Try
            .OfAsync(async () => await messageBus.DispatchAsync(new HelloWorldCommand(message)))
            .ThenAsync(async () => await messageBus.DispatchAsync(new HelloWorldQuery()))
    ));

app.Run();


void UseNEvo(IServiceCollection serviceCollection)
{
    serviceCollection.AddSingleton<IMessageHandlerRegistry, MessageHandlerRegistry>(sp => {
        var registry = new MessageHandlerRegistry(sp, 
            CommandHandlerWrapperFactory.MessageHandlerOptions,
            EventHandlerWrapperFactory.MessageHandlerOptions,
            QueryHandlerWrapperFactory.MessageHandlerOptions
        );
        registry.Register<HelloWorldHandler>();
        return registry;
    });

    serviceCollection.AddSingleton<IMessageProcessor, MessageProcessor>();
    serviceCollection.AddSingleton<IMessageBus, SynchronousMessageBus>();
}

//TODO -- global filter insted of throw
TResult ToResponse<TResult>(Try<TResult> either)
{
    return either.Handle(success => success, exc => throw exc);
}


public record HelloWorldCommand(string Message) : Command;

public record HelloWorldEvent(string Message, SourceId SourceId) : Event(SourceId);
public record HelloWorldQuery() : Query<string>;


public class HelloWorldHandler : ICommandHandler<HelloWorldCommand>, IEventHandler<HelloWorldEvent>, IQueryHandler<HelloWorldQuery, string>
{
    public IMessageBus _messageBus;

    public HelloWorldHandler(IMessageBus messageBus)
    {
        _messageBus = messageBus;
    }

    public async Task<Try<Unit>> HandleAsync(HelloWorldCommand command)
    {
        await _messageBus.PublishAsync(new HelloWorldEvent(command.Message, SourceId.New(nameof(HelloWorldCommand), command.CreatedAt.ToString())));

        if (command.Message?.Equals("Hi", StringComparison.InvariantCultureIgnoreCase))
            return Try.Failure(new Exception("Don't say hi!"));

        return Try.Success();
    }

    public Task HandleAsync(HelloWorldEvent @event)
    {
        Console.WriteLine(@event.Message);
        return Task.CompletedTask;
    }

    public async Task<string> HandleAsync(HelloWorldQuery query)
    {
        return "Hello my friendo!";
    }
}
