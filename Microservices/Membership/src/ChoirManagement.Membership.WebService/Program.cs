using ChoirManagement.Bootstraping;
using ChoirManagement.Membership.Domain;

var builder = ChoirManagementMicroserviceBootstrap
                .CreateBuilder(args);

builder.Services
    .RegisterMembershipServices(builder.Configuration.GetRequiredSection("Membership"));

var app =  builder.BuildMicroservice()
    .RegisterMembershipHandlers()
    .RegisterMembershipApi("/api");

app.Run();


