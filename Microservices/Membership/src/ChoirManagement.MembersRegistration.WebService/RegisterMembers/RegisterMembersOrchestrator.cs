using ChoirManagement.Membership.Public.Messages.Commands;
using ChoirManagement.MembersRegistration.WebService.RegisterMembers.DTO;
using NEvo.Core;
using NEvo.CQRS.Messaging.Commands;
using NEvo.Monads;

namespace ChoirManagement.MembersRegistration.WebService.RegisterMembers;

public class RegisterMembersOrchestrator : IRegisterMembersOrchestrator
{
    private readonly ICommandDispatcher _commandDispatcher;

    public RegisterMembersOrchestrator(ICommandDispatcher commandDispatcher)
    {
        _commandDispatcher = Check.Null(commandDispatcher);
    }

    public async Task<Either<Exception, Unit>> RegisterMemberAsync(RegisterInput input)
        => await _commandDispatcher
                // step 1
                .DispatchAsync(new RegisterMember(input.MemberId, input.Form.PersonalData))
                // step 2
                .ThenAsync(async _ =>
                    await _commandDispatcher
                        .DispatchAsync(new CreateIdentity(input.Form.Identity))
                        // step 1 compensation
                        .OnFailureAsync(async _ => await _commandDispatcher.DispatchAsync(new DeleteMember(input.MemberId))
                    )
                );
}
