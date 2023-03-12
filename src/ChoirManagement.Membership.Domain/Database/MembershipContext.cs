using ChoirManagement.Membership.Domain.Aggregates;
using Microsoft.EntityFrameworkCore;
using NEvo.ValueObjects;
using System.Reflection.PortableExecutable;

namespace ChoirManagement.Membership.Domain.Database;

public class MembershipContext : DbContext
{

    public DbSet<Member> Members { get; set; }

    public MembershipContext(DbContextOptions<MembershipContext> options)
        : base(options)
    {

    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Member>(builder =>
        {
            // value object primary key
            builder.Property(x => x.Id).HasConversion(x => x.Id, l => l).HasColumnName("Id").IsRequired();

            builder.OwnsOne(x => x.PersonalData, personalDataBuilder =>
            {
                // Date is a DateOnly property and date on database
                personalDataBuilder.Property(x => x.BirthDate)
                    .HasConversion<DateOnlyConverter, DateOnlyComparer>();

                // value objects
                personalDataBuilder.OwnsOne(x => x.Name);
                personalDataBuilder.OwnsOne(x => x.Address);
                personalDataBuilder.OwnsOne(x => x.PESEL);
                personalDataBuilder.OwnsOne(x => x.Email);
                personalDataBuilder.OwnsOne(x => x.PhoneNumber);
            });
        });
    }
}
