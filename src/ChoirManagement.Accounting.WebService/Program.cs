var instanceId = Guid.NewGuid();
var builder = WebApplication.CreateBuilder(args);
var app = builder.Build();

app.MapGet("/", () => $"Hello World from instance {instanceId}");

app.Run();