using ChoirManagement.Bootstraping;
using ChoirManagement.MembersRegistration.WebService.RegisterMembers;
using NEvo.Core;
using NEvo.CQRS.Messaging.Commands;

WebApplicationBuilder builder = ChoirManagementMicroserviceBootstrap
                .CreateBuilder(args);

builder.Services.AddScoped<IRegisterMembersOrchestrator, RegisterMembersOrchestrator>();

WebApplication app = builder.BuildMicroservice();

app.UseRegisterMembersRoutes();

app.Run();


//TODO: move to Identity server
public record CreateIdentity : Command
{
    public UserIdentity UserIdentity { get; set; }
    public CreateIdentity(UserIdentity userIdentity)
    {
        UserIdentity = Check.Null(userIdentity);
    }
}

public class UserIdentity
{
    public string? Login { get; set; }
    public string? Password { get; set; }
}