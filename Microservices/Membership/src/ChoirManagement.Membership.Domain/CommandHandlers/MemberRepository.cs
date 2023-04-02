using ChoirManagement.Membership.Domain.Aggregates;
using ChoirManagement.Membership.Domain.Database;
using ChoirManagement.Membership.Domain.Repositories;
using ChoirManagement.Membership.Public.ValueObjects;
using NEvo.Core;
using NEvo.CQRS.Messaging.Events;
using NEvo.Monads;

namespace ChoirManagement.Membership.Domain.CommandHandlers;

/// <summary>
/// only mock
/// </summary>
public class MemberRepository : IMemberRepository
{
    private MembershipContext _context;
    private IEventPublisher _eventPublisher;

    public MemberRepository(MembershipContext context, IEventPublisher eventPublisher)
    {
        _context = context;
        _eventPublisher = eventPublisher;
    }
    public Maybe<Member> Get(MemberId memberId) => GetAsync(memberId).GetAwaiter().GetResult();

    public async Task<Maybe<Member>> GetAsync(MemberId memberId) => await _context.Members.FindAsync(memberId);

    public void Save(Member member)
    {
        SaveAsync(member).GetAwaiter().GetResult();
    }

    public async Task SaveAsync(Member member)
    {
        var entry = _context.Entry(member);
        if (entry.State == Microsoft.EntityFrameworkCore.EntityState.Detached)
        {
            await _context.AddAsync(member);
        }
        else
        {
            entry.State = Microsoft.EntityFrameworkCore.EntityState.Modified;
        }
        await _context.SaveChangesAsync();

        //todo: move to commit
        foreach(var @event in member.FlushEvents())
        {
            await _eventPublisher.PublishAsync(@event);
        }
    }
}