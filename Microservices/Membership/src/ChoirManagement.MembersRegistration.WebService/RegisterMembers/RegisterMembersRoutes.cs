using ChoirManagement.MembersRegistration.WebService.RegisterMembers.DTO;
using Microsoft.AspNetCore.Mvc;
using NEvo.Core.Web;

namespace ChoirManagement.MembersRegistration.WebService.RegisterMembers;

public static class RegisterMembersRoutes
{
    public static TApplication UseRegisterMembersRoutes<TApplication>(this TApplication application) where TApplication : IEndpointRouteBuilder
    {
        application.MapPost("/api/members/register", async ([FromBody] RegisterInput input, IRegisterMembersOrchestrator service) =>
            await service.RegisterMemberAsync(input).ToResults()
        );
        return application;
    }
}
