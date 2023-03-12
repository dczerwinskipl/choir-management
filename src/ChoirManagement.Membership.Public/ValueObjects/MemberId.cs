using NEvo.ValueObjects;
using Newtonsoft.Json;

namespace ChoirManagement.Membership.Public.ValueObjects;

public class MemberId : Identifier<MemberId, Guid>
{
    [JsonConstructor]
    private MemberId(Guid id) : base(id)
    {
    }

    private MemberId() : base() { }

    public static MemberId New() => new MemberId(Guid.NewGuid());

    public static MemberId New(Guid id) => new MemberId(id);

    public override MemberId Copy() => new MemberId(Id);

    public static implicit operator Guid(MemberId memberId) => memberId.Id;

    public static implicit operator MemberId(Guid id) => new MemberId(id);
}