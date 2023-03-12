using NEvo.ValueObjects;
using Newtonsoft.Json;

namespace ChoirManagement.Membership.Public.ValueObjects;

public class MembershipId : Identifier<MembershipId, Guid>
{
    [JsonConstructor]
    private MembershipId(Guid id) : base(id)
    {
    }

    private MembershipId() : base() { }

    public static MembershipId New() => new MembershipId(Guid.NewGuid());

    public static MembershipId New(Guid id) => new MembershipId(id);

    public override MembershipId Copy() => new MembershipId(Id);

    public static implicit operator Guid(MembershipId MembershipId) => MembershipId.Id;

    public static implicit operator MembershipId(Guid id) => new MembershipId(id);
}