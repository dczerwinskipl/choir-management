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


    public async Task<Try<Unit>> HandleAsync(RegisterMember command)
        => await Try
                    .OfAsync(async () => await _repository.GetAsync(command.MemberId))
                    // i tutaj lepszy byłby właśnie either/Optional, etc. ale spróbujemy z Try
                    .HandleAsync(
                        memberFound => Try.TaskSuccess(),
                        async memberNotFound => await Try
                                        .Of(() => Member.NewMember(command.MemberRegistrationForm, command.MemberId))
                                        .ThenAsync(_repository.SaveAsync)
                    );

    public async Task<Try<Unit>> HandleAsync(AnonimizeMember command)
        => await Try
                    .OfAsync(async () => await _repository.GetAsync(command.MemberId))
                    .ThenAsync(member => member.Anonimize().Map(_ => member))
                    .ThenAsync(_repository.SaveAsync)
                    .Catch(exception => exception switch
                    {
                        MemberAlreadyAnonymisedException => Try.Success(),
                        _ => Try.Failure(exception)
                    });

}
