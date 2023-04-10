using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.OpenApi.Any;
using Microsoft.OpenApi.Models;
using NEvo.Core;
using NEvo.Core.Reflection;
using NEvo.CQRS.Web;
using NEvo.ValueObjects;
using NEvo.Web.Middlewares;
using Swashbuckle.AspNetCore.SwaggerGen;
using System.Net;
using System.Reflection;
using System.Text;
using System.Text.Json;

namespace ChoirManagement.Bootstraping;

public static class ChoirManagementMicroserviceBootstrap
{
    /// <summary>
    /// TODO: change to NEvo API if available
    /// </summary>
    /// <param name="args"></param>
    public static WebApplicationBuilder CreateBuilder(string[] args)
    {
        WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

        //configuration
        _ = builder.Configuration.AddEnvironmentVariables()
                             .SetBasePath(Directory.GetCurrentDirectory())
                             .AddJsonFile("appsettings.json")
                             .AddJsonFiles("secrets/*.secrets.json", true)
                             .AddUserSecrets(Assembly.GetExecutingAssembly(), true);

        // preload all assemblies
        Directory
            .GetFiles(AppDomain.CurrentDomain.BaseDirectory, "*.dll")
            .ForEach(file => Assembly.LoadFrom(file));

        var choirManagementAssemblies =
            AppDomain.CurrentDomain.GetAssemblies().Where(a => a.FullName?.StartsWith("ChoirManagement") ?? false).ToList();

        // nevo
        IConfigurationSection cqrsConfiguration = builder.Configuration.GetRequiredSection("NEvo.CQRS");
        builder.Services
            .AddNEvo(nEvo => nEvo
                .AddCqrs(cqrs => cqrs
                                    .ConfigureRoutingPolicy(options => cqrsConfiguration.GetRequiredSection("Endpoint:RoutingPolicy").Bind(options))
                                    .ConfigureRoutingTopology(options => cqrsConfiguration.GetRequiredSection("Topology").Bind(options))
                )
                .AddMessagePoller(options => builder.Configuration.GetRequiredSection("MessagePoller").Bind(options))
                .AddAzureServiceBus(options => builder.Configuration.GetRequiredSection("AzureServiceBus:ClientData").Bind(options))
            )
            .Configure<TypeResolverOptions>(options =>
            {
                options.Assemblies.AddRange(choirManagementAssemblies);
            })

            // swagger
            .AddEndpointsApiExplorer()
            .AddSwaggerGen(options =>
            {
                options.SchemaFilter<ValueObjectSchemaFilter>();
            });

        return builder;
    }

    public static WebApplication BuildMicroservice(this WebApplicationBuilder builder)
    {
        WebApplication app = builder.Build();
        app
            // nevo
            .UseDebugConfigurationMiddleware()
            .UseCQRSEndpoint()
            // TODO: move to NEvo
            .UseExceptionHandler(errorApp =>
            {
                errorApp.Run(async context =>
                {
                    var MessageWithInner = (Exception? ex) =>
                    {
                        StringBuilder sb = new();
                        while (ex != null)
                        {
                            sb.AppendLine(ex.Message);
                            ex = ex.InnerException;
                        }
                        return sb.ToString();
                    };
                    Exception ex = context.Features.GetRequiredFeature<IExceptionHandlerFeature>().Error;

                    if (ex != null && ex is ArgumentException argumentException)
                    {
                        context.Response.ContentType = "application/json";
                        context.Response.StatusCode = (int)HttpStatusCode.BadRequest;

                        var response = new
                        {
                            ex.Message,
                            ValidationResult = new[]
                            {
                                new { argumentException.ParamName, argumentException.Message }
                            }
                        };

                        string json = JsonSerializer.Serialize(response);
                        await context.Response.WriteAsync(json);
                    }
                    else if (ex != null && ex is BadHttpRequestException badHttpRequestException)
                    {
                        context.Response.ContentType = "application/json";
                        context.Response.StatusCode = (int)HttpStatusCode.BadRequest;

                        var response = new
                        {
                            ex.Message,
                            ValidationResult = new[]
                            {
                                new { ParamName = "request", Message = MessageWithInner(ex) }
                            }
                        };

                        string json = JsonSerializer.Serialize(response);
                        await context.Response.WriteAsync(json);
                    }
                });
            })
            //swagger
            .UseSwagger()
            .UseSwaggerUI(options =>
            {
                options.SwaggerEndpoint("/swagger/v1/swagger.json", "v1");
                options.RoutePrefix = string.Empty;
            });

        return app;
    }
}


public class ValueObjectSchemaFilter : ISchemaFilter
{
    public void Apply(OpenApiSchema schema, SchemaFilterContext context)
    {
        var type = context.Type;
        var valueObjectMetadata = type.GetCustomAttribute<ValueObjectAttribute>();
        if (valueObjectMetadata != null)
        {
            if (valueObjectMetadata.IsSingleValue)
            {
                schema.Properties.Clear();
                schema.Format = valueObjectMetadata.Format;

                //todo: check underlying type
                schema.Type = "string";
                if (valueObjectMetadata.DefaultValue != null)
                {
                    //todo: check if its nullable?
                    schema.Default = new OpenApiString(valueObjectMetadata.DefaultValue);
                }
            }
        }
    }
}