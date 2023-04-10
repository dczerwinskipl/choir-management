using ChoirManagement.MembersRegistration.WebService.RegisterMembers.DTO;
using NEvo.Core;
using NEvo.Monads;

namespace ChoirManagement.MembersRegistration.WebService.RegisterMembers;

public interface IRegisterMembersOrchestrator
{
    Task<Either<Exception, Unit>> RegisterMemberAsync(RegisterInput input);
}
