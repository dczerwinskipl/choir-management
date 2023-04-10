using NEvo.ValueObjects;
using System.Text.Json.Serialization;

namespace ChoirManagement.Membership.Public.ValueObjects;

public class MembershipId : Identifier<MembershipId, Guid>
{
    [JsonConstructor]
    private MembershipId(Guid id) : base(id)
    {
    }

    private MembershipId() : base() { }

    public static MembershipId New() => new(Guid.NewGuid());

    public static MembershipId New(Guid id) => new(id);

    public override MembershipId Copy() => new(Id);

    public static implicit operator Guid(MembershipId MembershipId) => MembershipId.Id;

    public static implicit operator MembershipId(Guid id) => new(id);
}