using ChoirManagement.Membership.Domain.Aggregates;
using ChoirManagement.Membership.Domain.Repositories;
using ChoirManagement.Membership.Public.Messages.Commands;
using NEvo.Core;
using NEvo.CQRS.Processing.Commands;
using NEvo.Monads;

namespace ChoirManagement.Membership.Domain.CommandHandlers;

public class MemberCommandHandlers :
        ICommandHandler<RegisterMember>,
        ICommandHandler<AnonimizeMember>,
        ICommandHandler<DeleteMember>
{
    private readonly IMemberRepository _repository;

    public MemberCommandHandlers(IMemberRepository repository)
    {
        _repository = repository;
    }


    public async Task<Either<Exception, Unit>> HandleAsync(RegisterMember command)
        => await _repository
                    .GetAsync(command.MemberId)
                    .MatchAsync(
                        none: () => Member
                                        .NewMember(command.PersonalData, command.MemberId)
                                        .ThenAsync(_repository.SaveAsync),
                        some: Either.TaskSuccess
                    );

    public async Task<Either<Exception, Unit>> HandleAsync(DeleteMember command)
       => await _repository
                    .GetAsync(command.MemberId)
                    .MatchAsync(
                        // TODO: allow to ommit Try, its ugly
                        some: async member => await Try.OfAsync(async () => await _repository.DeleteAsync(member)),
                        none: Either.TaskSuccess
                    );

    public async Task<Either<Exception, Unit>> HandleAsync(AnonimizeMember command)
        => await _repository
                    .GetAsync(command.MemberId)
                    .MatchAsync(
                        some: member => member.Anonimize()
                                              .ThenAsync(() => _repository.SaveAsync(member))
                                              .Handle(exception => exception switch
                                              {
                                                  MemberAlreadyAnonymisedException => Either.Success(),
                                                  _ => Either.Failure(exception)
                                              }),
                        none: () => new NotFoundException(command.MemberId)
                    );
}
