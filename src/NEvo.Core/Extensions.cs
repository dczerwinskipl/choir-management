using Microsoft.Extensions.DependencyInjection;
using NEvo.Core.DomainDrivenDesign;
using NEvo.Messaging;
using NEvo.Processing.Registering;
using NEvo.Processing;
using System.ComponentModel.DataAnnotations;
using System.Net;
using System.Reflection;
using NEvo.Processing.Commands;
using NEvo.Processing.Events;
using NEvo.Processing.Queries;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;

/* all of that should be in NEvo MVC integration project */
namespace NEvo.Core;

public static class IntegrationExtensions
{
    public static void UseNEvo(this WebApplication application, params (string Prefix, Action<RouteGroupBuilder> ConfigureRouteGroup)[] routeGroups)
    {
        foreach(var (prefix, configureRouteGroup) in routeGroups)
        {
            var group = application.MapGroup(prefix);
            group.AddEndpointFilter<TryEndpointFilter>();
            configureRouteGroup(group);
        }
    }

    public static void AddNEvo(this WebApplicationBuilder builder, Action<MessageHandlerRegistry>? configureHandlers = null)
    {
        AddNEvo(builder.Services, configureHandlers);
    }

    public static void AddNEvo(this IServiceCollection serviceCollection, Action<MessageHandlerRegistry>? configureHandlers = null)
    {
        serviceCollection.AddSingleton<IMessageHandlerRegistry, MessageHandlerRegistry>(sp =>
        {
            var registry = new MessageHandlerRegistry(sp,
                CommandHandlerWrapperFactory.MessageHandlerOptions,
                EventHandlerWrapperFactory.MessageHandlerOptions,
                QueryHandlerWrapperFactory.MessageHandlerOptions
            );
            configureHandlers?.Invoke(registry);
            return registry;
        });

        serviceCollection.AddSingleton<IMessageProcessor, MessageProcessor>();
        serviceCollection.AddSingleton<IMessageBus, SynchronousMessageBus>();
    }
}

public class TryEndpointFilter : IEndpointFilter
{
    public static MethodInfo _invokeGeneric = Check.Null(typeof(TryEndpointFilter).GetMethod(nameof(MapTryResult), BindingFlags.NonPublic | BindingFlags.Instance));
    public async ValueTask<object?> InvokeAsync(EndpointFilterInvocationContext context, EndpointFilterDelegate next)
    {
        var result = await next(context);
        if (result == null)
            return result;

        var type = result.GetType();
        if (type.IsGenericType && type.GetGenericTypeDefinition() == typeof(Try<>))
        {
            var resultType = type.GetGenericArguments()[0];
            return _invokeGeneric.MakeGenericMethod(resultType).Invoke(this, new[] { result, context });
        }

        return result;
    }

    private object? MapTryResult<TTryResult>(Try<TTryResult> rawResponse, EndpointFilterInvocationContext context) =>
        rawResponse.Handle<object?>(
            success => success,
            exception =>
            {
                context.HttpContext.Response.StatusCode = ToHttpCode(exception);
                return new
                {
                    exception.Message, // todo: can add translation here or sth
                                       // todo: validations, etc.? need some mapper for exception to do that
                    Exception = exception
                };
            });

    //todo: extension of this method, mapping exception type to HTTP Codes, now its only to show basic contept
    public int ToHttpCode(Exception exception) => exception switch
    {
        DomainException or ValidationException => (int)HttpStatusCode.BadRequest,
        _ => (int)HttpStatusCode.InternalServerError,
    };
}