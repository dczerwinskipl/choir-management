using ChoirManagement.Membership.Public.ValueObjects;
using NEvo.DomainDrivenDesign;

namespace ChoirManagement.Membership.Domain.Aggregates;

public class Membership : SnapshotAggregateRoot<Membership, MembershipId>
{

#pragma warning disable CS8618 // ORM only
    internal Membership() : base() { }
#pragma warning restore CS8618 // ORM only

    internal Membership(MembershipId id) : base(id)
    {
    }
}