using ChoirManagement.Membership.Domain.Aggregates;
using ChoirManagement.Membership.Domain.Repositories;
using ChoirManagement.Membership.Public.Messages.Commands;
using NEvo.Core;
using NEvo.Processing.Commands;

namespace ChoirManagement.Membership.Domain.CommandHandlers;

public class MemberCommandHandlers :
        ICommandHandler<RegisterMember>,
        ICommandHandler<AnonimizeMember>
{
    private readonly IMemberRepository _repository;

    public MemberCommandHandlers(IMemberRepository repository)
    {
        _repository = repository;
    }


    public async Task<Either<Exception, Unit>> HandleAsync(RegisterMember command)
        => await Try
                    .OfAsync(async () => await _repository.GetAsync(command.MemberId), notFound => new NotFoundException(command.MemberId))
                    // i tutaj lepszy byłby właśnie either/Optional, etc. ale spróbujemy z Try
                    .MapAsync(
                        memberFound => Either.TaskRight(),
                        memberNotFound => Try
                                            .Of(() => Member.NewMember(command.MemberRegistrationForm, command.MemberId))
                                            .ThenAsync(_repository.SaveAsync)
                                            .ToUnit()
                    );

    public async Task<Either<Exception, Unit>> HandleAsync(AnonimizeMember command)
        => await Try
                    .OfAsync(async () => await _repository.GetAsync(command.MemberId), notFound => new NotFoundException(command.MemberId))
                    .Then(member => member.Anonimize())
                    .ThenAsync(_repository.SaveAsync)
                    .ToUnit()
                    .Handle(exception => exception switch
                    {
                        MemberAlreadyAnonymisedException => Either.Right(),
                        _ => Either.Left(exception)
                    });

}
