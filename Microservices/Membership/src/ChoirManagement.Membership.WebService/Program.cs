using ChoirManagement.Bootstraping;
using ChoirManagement.Membership.Domain;
using System.Reflection;

var builder = ChoirManagementMicroserviceBootstrap
                .CreateBuilder(args);

builder.Services
    .RegisterMembershipServices(builder.Configuration.GetRequiredSection("Membership"), Assembly.GetExecutingAssembly().FullName);

var app = builder.BuildMicroservice()
    .RegisterMembershipHandlers()
    .RegisterMembershipApi("/api");

app.Run();