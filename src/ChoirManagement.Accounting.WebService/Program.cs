using ChoirManagement.Accounting.WebService;
using NEvo.Core;

var instanceId = Guid.NewGuid();

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.AddNEvo(AccountingModuleConfiguration.Handlers); /* todo: modify API, should add handlers in app UseNEvo*/

var app = builder.Build();
app.UseNEvo(
    ("/api/settlements", AccountingModuleConfiguration.SettlementsRoutes)
    //("/api/payments", AccountingModuleConfiguration.PaymentsRoutes)
);

//TODO: only on developer Environment, but i have to modify pipelines
app.UseSwagger();
app.UseSwaggerUI(options =>
{
    options.SwaggerEndpoint("/swagger/v1/swagger.json", "v1");
    options.RoutePrefix = string.Empty;
});

app.Run();