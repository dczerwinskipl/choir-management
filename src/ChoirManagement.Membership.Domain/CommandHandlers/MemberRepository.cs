﻿using ChoirManagement.Membership.Domain.Aggregates;
using ChoirManagement.Membership.Domain.Database;
using ChoirManagement.Membership.Domain.Repositories;
using ChoirManagement.Membership.Public.ValueObjects;
using NEvo.Core;

namespace ChoirManagement.Membership.Domain.CommandHandlers;

/// <summary>
/// only mock
/// </summary>
public class MemberRepository : IMemberRepository
{
    private MembershipContext _context;

    public MemberRepository(MembershipContext context)
    {
        _context = context;
    }
    public Either<Null, Member> Get(MemberId memberId)
    {
        return GetAsync(memberId).GetAwaiter().GetResult();
    }

    public async Task<Either<Null, Member>> GetAsync(MemberId memberId)
    {
        var member = await _context.Members.FindAsync(memberId);
        return member is not null ? Either.Right<Null, Member>(member) : Either.Left<Null, Member>(Null.Value);
    }

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
    }
}