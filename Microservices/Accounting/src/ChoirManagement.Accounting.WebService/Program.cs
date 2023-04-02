using ChoirManagement.Accounting.WebService;
using NEvo.Core;
using NEvo.CQRS.Messaging;
using System.Reflection;

var builder = WebApplication.CreateBuilder(args);
builder.Configuration.AddEnvironmentVariables()
                     .SetBasePath(Directory.GetCurrentDirectory())
                     .AddJsonFile("appsettings.json")
                     .AddUserSecrets(Assembly.GetExecutingAssembly(), true);

builder.Services.AddNEvo(nEvo => nEvo
                                    .AddCqrs<RouterBasedMessageBus>()
                                    //.AddMessagePoller(options => builder.Configuration.GetRequiredSection("MessagePoller").Bind(options))
                                    //.AddAzureServiceBus(options => builder.Configuration.GetRequiredSection("AzureServiceBus:ClientData").Bind(options)
                                    //)
                        );
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();
app.UseNEvoCqrs(
    AccountingModuleConfiguration.Handlers
);

app.UseNEvoRoute(
    ("/api/settlements", AccountingModuleConfiguration.SettlementsRoutes)
    //("/api/payments", AccountingModuleConfiguration.PaymentsRoutes)
);

app.UseSwagger();
app.UseSwaggerUI(options =>
{
    options.SwaggerEndpoint("/swagger/v1/swagger.json", "v1");
    options.RoutePrefix = string.Empty;
});

await app.RunAsync();