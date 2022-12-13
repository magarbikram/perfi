﻿// <auto-generated />
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;
using Perfi.Infrastructure.Database;

#nullable disable

namespace Perfi.Infrastructure.Migrations
{
    [DbContext(typeof(AppDbContext))]
    partial class AppDbContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "7.0.0")
                .HasAnnotation("Relational:MaxIdentifierLength", 63);

            NpgsqlModelBuilderExtensions.UseIdentityByDefaultColumns(modelBuilder);

            modelBuilder.Entity("Perfi.Core.Accounts.AccountAggregate.Account", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<string>("AccountCategory")
                        .IsRequired()
                        .HasMaxLength(20)
                        .HasColumnType("character varying(20)");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(150)
                        .HasColumnType("character varying(150)");

                    b.Property<string>("Number")
                        .IsRequired()
                        .HasMaxLength(20)
                        .HasColumnType("character varying(20)");

                    b.Property<string>("ParentAccountNumber")
                        .HasMaxLength(20)
                        .HasColumnType("character varying(20)");

                    b.Property<string>("Type")
                        .IsRequired()
                        .HasMaxLength(13)
                        .HasColumnType("character varying(13)");

                    b.HasKey("Id");

                    b.HasIndex("AccountCategory");

                    b.HasIndex("Number")
                        .IsUnique();

                    b.HasIndex("ParentAccountNumber");

                    b.ToTable("Account", (string)null);

                    b.HasDiscriminator<string>("Type").HasValue("Account");

                    b.UseTphMappingStrategy();
                });

            modelBuilder.Entity("Perfi.Core.Accounts.CashAccountAggregate.CashAccount", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<string>("AssociatedAccountNumber")
                        .IsRequired()
                        .HasMaxLength(20)
                        .HasColumnType("character varying(20)");

                    b.Property<string>("BankName")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("character varying(100)");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(150)
                        .HasColumnType("character varying(150)");

                    b.HasKey("Id");

                    b.ToTable("CashAccount", (string)null);
                });

            modelBuilder.Entity("Perfi.Core.Accounts.CreditCardAggregate.CreditCardAccount", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<string>("AssociatedAccountNumber")
                        .IsRequired()
                        .HasMaxLength(20)
                        .HasColumnType("character varying(20)");

                    b.Property<string>("CreditorName")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("character varying(100)");

                    b.Property<string>("LastFourDigits")
                        .IsRequired()
                        .HasMaxLength(4)
                        .HasColumnType("character varying(4)");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(150)
                        .HasColumnType("character varying(150)");

                    b.HasKey("Id");

                    b.ToTable("CreditCardAccount", (string)null);
                });

            modelBuilder.Entity("Perfi.Core.Accounts.AccountAggregate.SummaryAccount", b =>
                {
                    b.HasBaseType("Perfi.Core.Accounts.AccountAggregate.Account");

                    b.ToTable("Account", (string)null);

                    b.HasDiscriminator().HasValue("Summary");
                });

            modelBuilder.Entity("Perfi.Core.Accounts.AccountAggregate.TransactionalAccount", b =>
                {
                    b.HasBaseType("Perfi.Core.Accounts.AccountAggregate.Account");

                    b.HasDiscriminator().HasValue("Transactional");
                });

            modelBuilder.Entity("Perfi.Core.Accounts.AccountAggregate.Account", b =>
                {
                    b.HasOne("Perfi.Core.Accounts.AccountAggregate.SummaryAccount", null)
                        .WithMany("Components")
                        .HasForeignKey("ParentAccountNumber")
                        .HasPrincipalKey("Number");
                });

            modelBuilder.Entity("Perfi.Core.Accounts.AccountAggregate.SummaryAccount", b =>
                {
                    b.Navigation("Components");
                });
#pragma warning restore 612, 618
        }
    }
}