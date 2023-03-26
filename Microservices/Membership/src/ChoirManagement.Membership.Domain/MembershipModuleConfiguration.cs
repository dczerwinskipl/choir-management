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
using Microsoft.Extensions.DependencyInjection;
using NEvo.Core;
using NEvo.Monads;
using NEvo.Messaging;
using NEvo.Processing.Registering;

namespace ChoirManagement.Membership.Domain;

public class MembershipModuleConfiguration
{
    public static void RegisterServices(IServiceCollection services)
    {
        services.AddScoped<IMemberRepository, MemberRepository>();
        services.AddDbContext<MembershipContext>(c => c.UseSqlServer("Data Source=ext-sql-server;Initial Catalog=ChoirManagement.Memebership;Integrated Security=false;User ID=test;Password=test123;Connect Timeout=30;Encrypt=False;Trust Server Certificate=True;Application Intent=ReadWrite;Multi Subnet Failover=False", b => b.MigrationsAssembly("ChoirManagement.Membership.WebService")));
        //services.AddDbContext<Member>
    }

    public static void Handlers(IMessageHandlerRegistry registry)
    {
        registry.Register<MemberCommandHandlers>();
    }

    public static void MemberRoutes(RouteGroupBuilder builder)
    {
        builder.MapPost("/", async ([FromServices] IMessageBus messageBus, [FromBody]MemberRegistrationForm form) =>
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
