using System.ComponentModel.DataAnnotations;
using System.Net;
using System.Reflection;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using NEvo.Monads;

/* all of that should be in NEvo MVC integration project */
namespace NEvo.Core;

public static class IntegrationExtensions
{
    public static WebApplication UseNEvoRoute(this WebApplication application, params (string Prefix, Action<RouteGroupBuilder> ConfigureRouteGroup)[] routeGroups)
    {
        foreach(var (prefix, configureRouteGroup) in routeGroups)
        {
            var group = application.MapGroup(prefix);
            group.AddEndpointFilter<TryEndpointFilter>();
            configureRouteGroup(group);
        }
        return application;
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
        if (type.IsGenericType && type.GetGenericTypeDefinition() == typeof(Either<,>))
        {
            var resultType = type.GetGenericArguments()[1];
            return _invokeGeneric.MakeGenericMethod(resultType).Invoke(this, new[] { result, context });
        }

        return result;
    }

    private object? MapTryResult<TTryResult>(Either<Exception, TTryResult> rawResponse, EndpointFilterInvocationContext context) =>
        rawResponse.Map<object?>(
            success => success,
            exception =>
            {
                context.HttpContext.Response.StatusCode = ToHttpCode(exception);
                return new
                {
                    exception.Message, // todo: can add translation here or sth
                                       // todo: validations, etc.? need some mapper for exception to do that
                    Exception = exception.ToString()
                };
            });

    //todo: extension of this method, mapping exception type to HTTP Codes, now its only to show basic contept
    public int ToHttpCode(Exception exception) => exception switch
    {
        ArgumentException or ValidationException => (int)HttpStatusCode.BadRequest,
        _ => (int)HttpStatusCode.InternalServerError,
    };
}