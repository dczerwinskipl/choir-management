using ChoirManagement.Accounting.Domain.Handlers;
using ChoirManagement.Accounting.Messages;
using Microsoft.AspNetCore.Mvc;
using NEvo.Core;
using NEvo.Messaging;
using NEvo.Processing.Registering;

namespace ChoirManagement.Accounting.WebService;

public static class AccountingModuleConfiguration
{
    public static void Handlers(IMessageHandlerRegistry registry)
    {
        registry.Register<HelloWorldHandler>();
    }

    public static void SettlementsRoutes(RouteGroupBuilder builder)
    {
        builder.MapGet("", async () => { Console.WriteLine("Test"); return Either.Right("Hello world"); })
               .Produces(200, typeof(string))
               .Produces(400, typeof(Unit))
               .Produces(500, typeof(Unit));

        builder.MapGet("/{message}", async ([FromServices] IMessageBus messageBus, [FromRoute] string message) => await Try
                        .OfAsync(async () => await messageBus.DispatchAsync(new HelloWorldCommand(message)))
                        .ThenAsync(async (_) => await messageBus.DispatchAsync(new HelloWorldQuery())))
                /* todo: should be handled by middleware when using Try return type*/
               .Produces(200, typeof(string))
               .Produces(400, typeof(Unit) /* Validation result output */)
               .Produces(500, typeof(Unit)/* Server error result output */);
    }
}
