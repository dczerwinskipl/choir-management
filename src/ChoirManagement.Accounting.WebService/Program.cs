using Microsoft.AspNetCore.Mvc;
using NEvo.Core;
using NEvo.Messaging;
using NEvo.Messaging.Commands;
using NEvo.Processing;
using NEvo.Processing.Commands;
using NEvo.Processing.Registering;

var instanceId = Guid.NewGuid();
var builder = WebApplication.CreateBuilder(args);
UseNEvo(builder.Services);

var app = builder.Build();

app.MapGet("/", () => $"Hello World from instance {instanceId}");
app.MapGet("/HelloWorld", async ([FromServices]IMessageBus messageBus) => ToResponse(await messageBus.DispatchAsync(new HelloWorldCommand($"Say hello!"))));

app.Run();



//TODO -- global handler?
TResult ToResponse<TResult>(Either<Exception, TResult> either)
{
    return either.Handle(success => success, exc => throw exc);
}

void UseNEvo(IServiceCollection serviceCollection)
{
    serviceCollection.AddSingleton<IMessageHandlerRegistry, MessageHandlerRegistry>(sp => {
        var registry = new MessageHandlerRegistry(sp, CommandHandlerWrapperFactory.MessageHandlerOptions);
        registry.Register<HelloWorldHandler>();
        return registry;
    });

    serviceCollection.AddSingleton<IMessageProcessor, MessageProcessor>();
    serviceCollection.AddSingleton<IMessageBus, SynchronousMessageBus>();
}

public record HelloWorldCommand(string Message) : Command;
public class HelloWorldHandler : ICommandHandler<HelloWorldCommand>
{
    public Task<Either<Exception, Unit>> HandleAsync(HelloWorldCommand command)
    {
        Console.WriteLine(command.Message);
        return Either.TaskRight();
    }
}