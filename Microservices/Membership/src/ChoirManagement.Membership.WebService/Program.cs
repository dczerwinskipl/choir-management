using System.Reflection;
using ChoirManagement.Membership.Domain;
using Microsoft.Extensions.Options;
using NEvo.Core;
using NEvo.CQRS.Routing;
using NEvo.Web.Middlewares;

var builder = WebApplication.CreateBuilder(args);

builder.Configuration.AddEnvironmentVariables()
                     .SetBasePath(Directory.GetCurrentDirectory())
                     .AddJsonFile("appsettings.json")
                     .AddUserSecrets(Assembly.GetExecutingAssembly(), true);

var cqrsConfiguration = builder.Configuration.GetRequiredSection("NEvo.CQRS");


builder.Services.AddNEvo(nEvo => nEvo
                                    .AddCqrs(/*cqrs => cqrs*/
                                                        //.ConfigureRoutingPolicy(options => cqrsConfiguration.GetRequiredSection("Endpoint:RoutingPolicy").Bind(options))
                                                        //.ConfigureRoutingTopology(options => cqrsConfiguration.GetRequiredSection("Topology").Bind(options))
                                    )
                                    //.AddMessagePoller(options => builder.Configuration.GetRequiredSection("MessagePoller").Bind(options))
                                    //.AddAzureServiceBus(options => builder.Configuration.GetRequiredSection("AzureServiceBus:ClientData").Bind(options))
                        );

//TODO: ³adniej
//MembershipModuleConfiguration.RegisterServices(builder.Services, builder.Configuration.GetRequiredSection("Membership"));

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();
var topology = app.Services.GetService<IOptions<RoutingTopologyDescription>>();
var policy = app.Services.GetService<IOptions<RoutingPolicyDescription>>();
app.UseNEvoCqrs(MembershipModuleConfiguration.Handlers);
app.UseNEvoRoute(
    ("/api/members", MembershipModuleConfiguration.MemberRoutes)
//("/api/payments", AccountingModuleConfiguration.PaymentsRoutes)
);

app.UseDebugConfigurationMiddleware();
app.UseSwagger();
app.UseSwaggerUI(options =>
{
    options.SwaggerEndpoint("/swagger/v1/swagger.json", "v1");
    options.RoutePrefix = string.Empty;
});

app.Run();


