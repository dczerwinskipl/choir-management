﻿// <auto-generated />
using System;
using ChoirManagement.Membership.Domain.Database;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace ChoirManagement.Membership.WebService.Migrations
{
    [DbContext(typeof(MembershipContext))]
    partial class MembershipContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "7.0.3")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

            modelBuilder.Entity("ChoirManagement.Membership.Domain.Aggregates.Member", b =>
                {
                    b.Property<Guid>("Id")
                        .HasColumnType("uniqueidentifier")
                        .HasColumnName("Id");

                    b.Property<bool>("IsAnonymised")
                        .HasColumnType("bit");

                    b.HasKey("Id");

                    b.ToTable("Members");
                });

            modelBuilder.Entity("ChoirManagement.Membership.Domain.Aggregates.Member", b =>
                {
                    b.OwnsOne("ChoirManagement.Membership.Public.ValueObjects.MemberPersonalData", "PersonalData", b1 =>
                        {
                            b1.Property<Guid>("MemberId")
                                .HasColumnType("uniqueidentifier");

                            b1.Property<DateTime>("BirthDate")
                                .HasColumnType("datetime2");

                            b1.HasKey("MemberId");

                            b1.ToTable("Members");

                            b1.WithOwner()
                                .HasForeignKey("MemberId");

                            b1.OwnsOne("NEvo.ValueObjects.PersonalData.Address", "Address", b2 =>
                                {
                                    b2.Property<Guid>("MemberPersonalDataMemberId")
                                        .HasColumnType("uniqueidentifier");

                                    b2.Property<string>("ApartmentNumber")
                                        .HasMaxLength(16)
                                        .HasColumnType("nvarchar(16)");

                                    b2.Property<string>("City")
                                        .IsRequired()
                                        .HasMaxLength(32)
                                        .HasColumnType("nvarchar(32)");

                                    b2.Property<string>("Commune")
                                        .HasMaxLength(32)
                                        .HasColumnType("nvarchar(32)");

                                    b2.Property<string>("County")
                                        .HasMaxLength(32)
                                        .HasColumnType("nvarchar(32)");

                                    b2.Property<string>("HouseNumber")
                                        .IsRequired()
                                        .HasMaxLength(16)
                                        .HasColumnType("nvarchar(16)");

                                    b2.Property<string>("PostalCode")
                                        .IsRequired()
                                        .HasMaxLength(6)
                                        .HasColumnType("nvarchar(6)");

                                    b2.Property<string>("Street")
                                        .IsRequired()
                                        .HasMaxLength(64)
                                        .HasColumnType("nvarchar(64)");

                                    b2.Property<string>("Voivodeship")
                                        .HasMaxLength(32)
                                        .HasColumnType("nvarchar(32)");

                                    b2.HasKey("MemberPersonalDataMemberId");

                                    b2.ToTable("Members");

                                    b2.WithOwner()
                                        .HasForeignKey("MemberPersonalDataMemberId");
                                });

                            b1.OwnsOne("NEvo.ValueObjects.PersonalData.Email", "Email", b2 =>
                                {
                                    b2.Property<Guid>("MemberPersonalDataMemberId")
                                        .HasColumnType("uniqueidentifier");

                                    b2.Property<string>("Value")
                                        .IsRequired()
                                        .HasMaxLength(255)
                                        .HasColumnType("nvarchar(255)");

                                    b2.HasKey("MemberPersonalDataMemberId");

                                    b2.ToTable("Members");

                                    b2.WithOwner()
                                        .HasForeignKey("MemberPersonalDataMemberId");
                                });

                            b1.OwnsOne("NEvo.ValueObjects.PersonalData.Name", "Name", b2 =>
                                {
                                    b2.Property<Guid>("MemberPersonalDataMemberId")
                                        .HasColumnType("uniqueidentifier");

                                    b2.Property<string>("FirstName")
                                        .IsRequired()
                                        .HasMaxLength(32)
                                        .HasColumnType("nvarchar(32)");

                                    b2.Property<string>("LastName")
                                        .IsRequired()
                                        .HasMaxLength(32)
                                        .HasColumnType("nvarchar(32)");

                                    b2.Property<string>("MiddleName")
                                        .HasMaxLength(32)
                                        .HasColumnType("nvarchar(32)");

                                    b2.HasKey("MemberPersonalDataMemberId");

                                    b2.ToTable("Members");

                                    b2.WithOwner()
                                        .HasForeignKey("MemberPersonalDataMemberId");
                                });

                            b1.OwnsOne("NEvo.ValueObjects.PersonalData.PESEL", "PESEL", b2 =>
                                {
                                    b2.Property<Guid>("MemberPersonalDataMemberId")
                                        .HasColumnType("uniqueidentifier");

                                    b2.Property<string>("Number")
                                        .IsRequired()
                                        .HasMaxLength(11)
                                        .HasColumnType("nvarchar(11)");

                                    b2.HasKey("MemberPersonalDataMemberId");

                                    b2.ToTable("Members");

                                    b2.WithOwner()
                                        .HasForeignKey("MemberPersonalDataMemberId");
                                });

                            b1.OwnsOne("NEvo.ValueObjects.PersonalData.PhoneNumber", "PhoneNumber", b2 =>
                                {
                                    b2.Property<Guid>("MemberPersonalDataMemberId")
                                        .HasColumnType("uniqueidentifier");

                                    b2.Property<string>("Value")
                                        .IsRequired()
                                        .HasMaxLength(32)
                                        .HasColumnType("nvarchar(32)");

                                    b2.HasKey("MemberPersonalDataMemberId");

                                    b2.ToTable("Members");

                                    b2.WithOwner()
                                        .HasForeignKey("MemberPersonalDataMemberId");
                                });

                            b1.Navigation("Address")
                                .IsRequired();

                            b1.Navigation("Email")
                                .IsRequired();

                            b1.Navigation("Name")
                                .IsRequired();

                            b1.Navigation("PESEL")
                                .IsRequired();

                            b1.Navigation("PhoneNumber")
                                .IsRequired();
                        });

                    b.Navigation("PersonalData")
                        .IsRequired();
                });
#pragma warning restore 612, 618
        }
    }
}
