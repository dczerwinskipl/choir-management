using NEvo.DomainDrivenDesign;
using NEvo.ValueObjects;
using NEvo.Core;
using ChoirManagement.Membership.Public.ValueObjects;
using ChoirManagement.Membership.Public.Messages.Events;
using NEvo.Monads;

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
    /// <param name="registrationForm"></param>
    /// <param name="id"></param>
    /// <returns></returns>
    public static Either<Exception, Member> NewMember(MemberRegistrationForm registrationForm, MemberId? id = null) 
        => Rule("Date of birth must be consistent with PESEL", () => registrationForm.PESEL.GetBirthDate() == registrationForm.BirthDate)
            .Rule("Date of birth must be after 1900", () => registrationForm.BirthDate > new DateOnly(1990, 1, 1))
            .OnSuccess(() => new Member(id ?? MemberId.New(), registrationForm));

}