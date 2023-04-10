using ChoirManagement.Membership.Public.Messages.Events;
using ChoirManagement.Membership.Public.ValueObjects;
using NEvo.Core;
using NEvo.DomainDrivenDesign;
using NEvo.Monads;
using NEvo.ValueObjects;

namespace ChoirManagement.Membership.Domain.Aggregates;

public class Member : SnapshotAggregateRoot<Member, MemberId>
{
    public MemberPersonalData PersonalData { get; private set; }
    public bool IsAnonymised { get; private set; }

    protected override ObjectId? GetSource() => Id.ToObjectId();

#pragma warning disable CS8618 // ORM only
    internal Member() : base() { }
#pragma warning restore CS8618 // ORM only

    internal Member(MemberId id, MemberPersonalData memberPersonalData) : base(id)
    {
        PersonalData = memberPersonalData with { };

        Publish(new MemberRegistered(Id));
        Publish(new MemberPersonalDataChanged(Id, PersonalData with { }));
    }

    /// <summary>
    /// Anonimize personal data of member
    /// </summary>
    /// <returns></returns>
    public Either<Exception, Unit> Anonimize()
            => Rule("The member cannot be already anonymised", () => !IsAnonymised, () => new MemberAlreadyAnonymisedException())
                //not implemented yet .Rule("The member must not have an active membership", () => !HasActiveMembership())
                .OnSuccess(() =>
                {
                    PersonalData = PersonalData.Anonimize();
                    IsAnonymised = true;
                    Publish(new MemberPersonalDataChanged(Id, PersonalData with { }));
                });

    /// <summary>
    /// Add new member
    /// </summary>
    /// <param name="personalData"></param>
    /// <param name="id"></param>
    /// <returns></returns>
    public static Either<Exception, Member> NewMember(MemberPersonalData personalData, MemberId? id = null)
        => Rule("Date of birth must be consistent with PESEL", () => personalData.PESEL.GetBirthDate() == personalData.BirthDate)
            .Rule("Date of birth must be after 1900", () => personalData.BirthDate > new DateOnly(1900, 1, 1))
            .OnSuccess(() => new Member(id ?? MemberId.New(), personalData));

}