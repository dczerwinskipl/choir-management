using ChoirManagement.Membership.Domain;
using NEvo.Core;
using NEvo.Messaging;
using System.Reflection;

var builder = WebApplication.CreateBuilder(args);

builder.Configuration.AddEnvironmentVariables()
                     .SetBasePath(Directory.GetCurrentDirectory())
                     .AddJsonFile("appsettings.json")
                     .AddUserSecrets(Assembly.GetExecutingAssembly(), true);

builder.Services.AddNEvo(nEvo => nEvo
                                    .AddCqrs<ExternalMessageBus>()
                                    .AddMessagePoller(options => builder.Configuration.GetRequiredSection("MessagePoller").Bind(options))
                                    .AddAzureServiceBus(options => builder.Configuration.GetRequiredSection("AzureServiceBus:ClientData").Bind(options))
                        );

//TODO: ³adniej
MembershipModuleConfiguration.RegisterServices(builder.Services);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();
app.UseNEvoCqrs(MembershipModuleConfiguration.Handlers);
app.UseNEvoRoute(
    ("/api/members", MembershipModuleConfiguration.MemberRoutes)
//("/api/payments", AccountingModuleConfiguration.PaymentsRoutes)
);

app.UseSwagger();
app.UseSwaggerUI(options =>
{
    options.SwaggerEndpoint("/swagger/v1/swagger.json", "v1");
    options.RoutePrefix = string.Empty;
});

app.Run();