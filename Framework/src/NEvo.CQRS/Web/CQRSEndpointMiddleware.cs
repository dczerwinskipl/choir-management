using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using NEvo.Core;
using NEvo.Core.Reflection;
using NEvo.CQRS.Messaging;
using NEvo.CQRS.Messaging.Commands;
using NEvo.CQRS.Processing;
using System.Text.Json;

namespace NEvo.CQRS.Web;

public static class CQRSMiddlewareExtensions
{
    public static TEndpointRouteBuilder UseCQRSEndpoint<TEndpointRouteBuilder>(this TEndpointRouteBuilder app) where TEndpointRouteBuilder : IEndpointRouteBuilder
    {
        app.MapPost("/api/Endpoint/DispatchCommand", async ([FromBody] MessageEnvelope messageEnvelope, IMessageProcessor messageProcessor, ITypeResolver typeResolver) =>
        {
            Type messageType;
            try
            {
                messageType = typeResolver.GetType(messageEnvelope.MessageType);
            }
            catch (Exception ex)
            {
                throw new ArgumentException($"Unable to resolve type: {messageEnvelope.MessageType}. {ex.Message}", nameof(messageEnvelope.MessageType), ex);
            }

            Command message;
            try
            {
                message = Check.Null(JsonSerializer.Deserialize(messageEnvelope.Payload, messageType) as Command);
            }
            catch (Exception ex)
            {
                throw new ArgumentException($"Unable to deserialize payload to type: {messageType.AssemblyQualifiedName}. {ex.Message}", nameof(messageEnvelope.Payload), ex);
            }

            return await messageProcessor
                            .ProcessAsync(message)
                            .ToResults();
        });
        return app;
    }
}