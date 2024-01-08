using ChoirManagement.Bootstraping;
using ChoirManagement.MembersRegistration.WebService.RegisterMembers;
using NEvo.CodeAnalysis.Analysing;
using NEvo.CodeAnalysis.Analysing.Extracting.Code;
using NEvo.CodeAnalysis.Analysing.Extracting.Web;
using NEvo.CodeAnalysis.Analysing.Processing;
using NEvo.CodeAnalysis.Web;
using NEvo.Core;
using NEvo.CQRS.Messaging.Commands;

WebApplicationBuilder builder = ChoirManagementMicroserviceBootstrap
                .CreateBuilder(args);

builder.Services.AddScoped<IRegisterMembersOrchestrator, RegisterMembersOrchestrator>();

builder.Services
    .AddCodeAnalyzer()
        .AddClassArtifactExtractor()
            .ConfigureClassArtifactExtractorNamespaces(typeof(MemberRegistrationConfiguration).Namespace, "NEvo.")
        .AddEndpointArtifactExtractor()
        .AddCallArtifactProcessor();

var app = builder.BuildMicroservice();

//app.UseRegisterMembersRoutes();
app.UseCodeDocumentation("/api/docs",
        typeof(MemberRegistrationConfiguration).Assembly,
        typeof(NEvo.CodeAnalysis.ICodeAnalyzer).Assembly,
        typeof(NEvo.CQRS.Web.CQRSMiddlewareExtensions).Assembly
    );

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