using ChoirManagement.Bootstraping;

var builder = ChoirManagementMicroserviceBootstrap
                .CreateBuilder(args);

var app = builder.BuildMicroservice();

app.Run();


