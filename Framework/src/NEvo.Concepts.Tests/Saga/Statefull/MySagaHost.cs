using System.Linq.Expressions;
using System.Reflection;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using NEvo.Core;
using NEvo.CQRS.Messaging.Events;
using NEvo.Monads;
using NEvo.Sagas.Stateful;
using NEvo.Sagas.Stateful.Handling;
using NEvo.ValueObjects;

namespace NEvo.Concepts.Tests.Saga.Statefull;

public static class MySagaHost
{
    public static async Task RunAsync(string[] args)
    {
        var app = CreateWebApplication(args);
        await app.StartAsync();

        var verificationId = Guid.NewGuid();
        var objectId = ObjectId.New("Verification", verificationId.ToString());

        var eventPublisher = app.Services.GetRequiredService<IEventPublisher>();
        await eventPublisher.PublishAsync(new MailVerificationStarted(objectId, verificationId));
        await eventPublisher.PublishAsync(new MailConfirmed(objectId, verificationId));

        Console.ReadLine();

    }

    public static WebApplication CreateWebApplication(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);
        builder.Configuration.AddEnvironmentVariables()
                             .SetBasePath(Directory.GetCurrentDirectory())
                             .AddJsonFile("appsettings.json", optional: true)
                             .AddUserSecrets(Assembly.GetExecutingAssembly(), true);

        builder.Services.AddNEvo(nEvo => nEvo
                                            .AddCqrs(c => c.UseCustomMessageHandlers(SagaStateMachineHandlerAdapterFactory.MessageHandlerOptions))
                                //.AddMessagePoller(options => builder.Configuration.GetRequiredSection("MessagePoller").Bind(options))
                                //.AddAzureServiceBus(options => builder.Configuration.GetRequiredSection("AzureServiceBus:ClientData").Bind(options)
                                //)
                                );
        builder.Services.AddEndpointsApiExplorer();

        builder.Services.AddSingleton<ISagaRrepository<MySaga, string>, InMemorySagaRrepository<MySaga, string>>();

        var app = builder.Build();
        app.UseNEvoCqrs(
            registry => registry.Register<MySagaHandler>()
        );

        return app;
    }
}


public class InMemorySagaRrepository<TSaga, TState> : ISagaRrepository<TSaga, TState> where TSaga : IStatefulSaga<TState>
{
    private readonly Dictionary<Guid, TSaga> _sagas = new Dictionary<Guid, TSaga>();
    public Task<Maybe<TSaga>> GetAsync(Expression<Func<TSaga, bool>> predicate) => Task.FromResult(_sagas.Values.Where(predicate.Compile()).MaybeSingle());

    public Task SaveAsync(TSaga newSagaContext)
    {
        _sagas[newSagaContext.Id] = newSagaContext;
        return Task.CompletedTask;
    }
}