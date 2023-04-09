using System.Reflection;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using NEvo.Core;
using NEvo.Web.Middlewares;

namespace ChoirManagement.Bootstraping;

public static class ChoirManagementMicroserviceBootstrap
{
    /// <summary>
    /// TODO: change to NEvo API if available
    /// </summary>
    /// <param name="args"></param>
    public static WebApplicationBuilder CreateBuilder(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        //configuration
        builder.Configuration.AddEnvironmentVariables()
                             .SetBasePath(Directory.GetCurrentDirectory())
                             .AddJsonFile("appsettings.json")
                             .AddJsonFiles("secrets/*.secrets.json", true)
                             .AddUserSecrets(Assembly.GetExecutingAssembly(), true);

        // nevo
        var cqrsConfiguration = builder.Configuration.GetRequiredSection("NEvo.CQRS");
        builder.Services.AddNEvo(nEvo => nEvo
                                            .AddCqrs(cqrs => cqrs
                                                                .ConfigureRoutingPolicy(options => cqrsConfiguration.GetRequiredSection("Endpoint:RoutingPolicy").Bind(options))
                                                                .ConfigureRoutingTopology(options => cqrsConfiguration.GetRequiredSection("Topology").Bind(options))
                                            )
                                            .AddMessagePoller(options => builder.Configuration.GetRequiredSection("MessagePoller").Bind(options))
                                            .AddAzureServiceBus(options => builder.Configuration.GetRequiredSection("AzureServiceBus:ClientData").Bind(options))
                                );

        // swagger
        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwaggerGen();

        return builder;
    }

    public static WebApplication BuildMicroservice(this WebApplicationBuilder builder)
    {
        var app = builder.Build();

        // nevo
        app.UseDebugConfigurationMiddleware();

        //swagger
        app.UseSwagger();
        app.UseSwaggerUI(options =>
        {
            options.SwaggerEndpoint("/swagger/v1/swagger.json", "v1");
            options.RoutePrefix = string.Empty;
        });

        return app;
    }
}
