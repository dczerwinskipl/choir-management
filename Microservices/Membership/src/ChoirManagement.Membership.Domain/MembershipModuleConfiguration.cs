using ChoirManagement.Membership.Domain.CommandHandlers;
using ChoirManagement.Membership.Domain.Database;
using ChoirManagement.Membership.Domain.Repositories;
using ChoirManagement.Membership.Public.Messages.Commands;
using ChoirManagement.Membership.Public.ValueObjects;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using NEvo.Core;
using NEvo.CQRS.Messaging;
using NEvo.Monads;

namespace ChoirManagement.Membership.Domain;

public static class MembershipModuleConfiguration
{
    public static void RegisterMembershipServices(this IServiceCollection services, IConfiguration configuration, string assemblyName)
    {
        services.AddScoped<IMemberRepository, MemberRepository>();
        services.AddDbContext<MembershipContext>(c => c
            .UseSqlServer(
                configuration.GetValue<string>("Database:ConnectionString"),
                b => b.MigrationsAssembly(assemblyName)
            )
        );
    }

    public static WebApplication RegisterMembershipHandlers(this WebApplication application)
    {
        application.UseNEvoCqrs(registry => registry.Register<MemberCommandHandlers>());
        return application;
    }

    public static WebApplication RegisterMembershipApi(this WebApplication application, string prefix = "/api")
    {
        application.UseNEvoRoute(
            ($"{prefix}/members", MemberRoutes),
            ($"{prefix}/memberships", MemberRoutes)
        );

        return application;
    }

    public static void MemberRoutes(RouteGroupBuilder builder)
    {
        builder.MapPost("/", async ([FromServices] IMessageBus messageBus, [FromBody] MemberPersonalData form) =>
                    await Try
                            .OfAsync(async () => await messageBus.DispatchAsync(new RegisterMember(MemberId.New(), form))))
               .Produces(200, typeof(Unit))
               .Produces(400, typeof(Unit))
               .Produces(500, typeof(Unit));

        builder.MapPost("/{memberId}/", async ([FromServices] IMessageBus messageBus, [FromRoute] Guid memberId) =>
                    await Try
                            .OfAsync(async () => await messageBus.DispatchAsync(new AnonimizeMember(MemberId.New(memberId)))))
               //.ThenAsync(async (_) => await messageBus.DispatchAsync(new GetMember())))
               /* todo: should be handled by middleware when using Try return type*/
               .Produces(200, typeof(Unit))
               .Produces(400, typeof(Unit) /* Validation result output */)
               .Produces(500, typeof(Unit)/* Server error result output */);
    }
}
