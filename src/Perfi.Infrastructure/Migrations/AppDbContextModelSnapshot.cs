﻿// <auto-generated />
using System;
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

            modelBuilder.Entity("Perfi.Core.Accounting.AccountingTransactionAggregate.AccountingEntry", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<string>("AccountNumber")
                        .IsRequired()
                        .HasMaxLength(20)
                        .HasColumnType("character varying(20)");

                    b.Property<int?>("AccountingTransactionId")
                        .HasColumnType("integer");

                    b.Property<DateTimeOffset>("DocumentDate")
                        .HasColumnType("timestamp with time zone");

                    b.Property<DateTimeOffset>("TransactionDate")
                        .HasColumnType("timestamp with time zone");

                    b.HasKey("Id");

                    b.HasIndex("AccountingTransactionId");

                    b.HasIndex("TransactionDate");

                    b.ToTable("AccountingEntry", (string)null);
                });

            modelBuilder.Entity("Perfi.Core.Accounting.AccountingTransactionAggregate.AccountingTransaction", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<string>("Description")
                        .IsRequired()
                        .HasMaxLength(255)
                        .HasColumnType("character varying(255)");

                    b.Property<DateTimeOffset>("DocumentDate")
                        .HasColumnType("timestamp with time zone");

                    b.Property<DateTimeOffset>("TransactionDate")
                        .HasColumnType("timestamp with time zone");

                    b.HasKey("Id");

                    b.HasIndex("TransactionDate");

                    b.ToTable("AccountingTransaction", (string)null);
                });

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

            modelBuilder.Entity("Perfi.Core.Accounts.LoanAggregate.Loan", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<string>("AssociatedAccountNumber")
                        .IsRequired()
                        .HasMaxLength(20)
                        .HasColumnType("character varying(20)");

                    b.Property<decimal>("InterestRate")
                        .HasColumnType("numeric");

                    b.Property<string>("LoanProvider")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("character varying(100)");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(150)
                        .HasColumnType("character varying(150)");

                    b.HasKey("Id");

                    b.ToTable("Loan", (string)null);
                });

            modelBuilder.Entity("Perfi.Core.Expenses.Expense", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<int?>("AccountingTransactionId")
                        .HasColumnType("integer");

                    b.Property<string>("Description")
                        .IsRequired()
                        .HasMaxLength(255)
                        .HasColumnType("character varying(255)");

                    b.Property<DateTimeOffset>("DocumentDate")
                        .HasColumnType("timestamp with time zone");

                    b.Property<string>("ExpenseCategoryCode")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("character varying(50)");

                    b.Property<DateTimeOffset>("TransactionDate")
                        .HasColumnType("timestamp with time zone");

                    b.Property<string>("TransactionPeriod")
                        .IsRequired()
                        .HasMaxLength(20)
                        .HasColumnType("character varying(20)");

                    b.HasKey("Id");

                    b.HasIndex("TransactionDate");

                    b.HasIndex("TransactionPeriod");

                    b.ToTable("Expense", (string)null);
                });

            modelBuilder.Entity("Perfi.Core.Expenses.ExpenseCategory", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<string>("AssociatedExpenseAccountNumber")
                        .IsRequired()
                        .HasMaxLength(20)
                        .HasColumnType("character varying(20)");

                    b.Property<string>("Code")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("character varying(50)");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(150)
                        .HasColumnType("character varying(150)");

                    b.Property<string>("Type")
                        .IsRequired()
                        .HasMaxLength(13)
                        .HasColumnType("character varying(13)");

                    b.HasKey("Id");

                    b.HasIndex("Code")
                        .IsUnique();

                    b.ToTable("ExpenseCategory", (string)null);

                    b.HasDiscriminator<string>("Type").HasValue("ExpenseCategory");

                    b.UseTphMappingStrategy();
                });

            modelBuilder.Entity("Perfi.Core.Expenses.ExpensePaymentMethod", b =>
                {
                    b.Property<int>("Id")
                        .HasColumnType("integer");

                    b.HasKey("Id");

                    b.ToTable("ExpensePaymentMethod", (string)null);

                    b.UseTptMappingStrategy();
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

            modelBuilder.Entity("Perfi.Core.Expenses.SummaryExpenseCategory", b =>
                {
                    b.HasBaseType("Perfi.Core.Expenses.ExpenseCategory");

                    b.ToTable("ExpenseCategory", (string)null);

                    b.HasDiscriminator().HasValue("Summary");
                });

            modelBuilder.Entity("Perfi.Core.Expenses.TransactionalExpenseCategory", b =>
                {
                    b.HasBaseType("Perfi.Core.Expenses.ExpenseCategory");

                    b.Property<string>("SummaryExpenseCategoryCode")
                        .IsRequired()
                        .HasColumnType("character varying(50)");

                    b.HasIndex("SummaryExpenseCategoryCode");

                    b.HasDiscriminator().HasValue("Transactional");
                });

            modelBuilder.Entity("Perfi.Core.Expenses.CashAccountExpensePaymentMethod", b =>
                {
                    b.HasBaseType("Perfi.Core.Expenses.ExpensePaymentMethod");

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

                    b.ToTable("CashAccountExpensePaymentMethod", (string)null);
                });

            modelBuilder.Entity("Perfi.Core.Expenses.CreditCardExpensePaymentMethod", b =>
                {
                    b.HasBaseType("Perfi.Core.Expenses.ExpensePaymentMethod");

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

                    b.ToTable("CreditCardExpensePaymentMethod", (string)null);
                });

            modelBuilder.Entity("Perfi.Core.Accounting.AccountingTransactionAggregate.AccountingEntry", b =>
                {
                    b.HasOne("Perfi.Core.Accounting.AccountingTransactionAggregate.AccountingTransaction", null)
                        .WithMany("AccountingEntries")
                        .HasForeignKey("AccountingTransactionId");

                    b.OwnsOne("Perfi.Core.Accounting.Money", "CreditAmount", b1 =>
                        {
                            b1.Property<int>("AccountingEntryId")
                                .HasColumnType("integer");

                            b1.Property<string>("Currency")
                                .HasMaxLength(4)
                                .HasColumnType("character varying(4)");

                            b1.Property<decimal?>("Value")
                                .HasColumnType("numeric");

                            b1.HasKey("AccountingEntryId");

                            b1.ToTable("AccountingEntry");

                            b1.WithOwner()
                                .HasForeignKey("AccountingEntryId");
                        });

                    b.OwnsOne("Perfi.Core.Accounting.Money", "DebitAmount", b1 =>
                        {
                            b1.Property<int>("AccountingEntryId")
                                .HasColumnType("integer");

                            b1.Property<string>("Currency")
                                .HasMaxLength(4)
                                .HasColumnType("character varying(4)");

                            b1.Property<decimal?>("Value")
                                .HasColumnType("numeric");

                            b1.HasKey("AccountingEntryId");

                            b1.ToTable("AccountingEntry");

                            b1.WithOwner()
                                .HasForeignKey("AccountingEntryId");
                        });

                    b.Navigation("CreditAmount")
                        .IsRequired();

                    b.Navigation("DebitAmount")
                        .IsRequired();
                });

            modelBuilder.Entity("Perfi.Core.Accounts.AccountAggregate.Account", b =>
                {
                    b.HasOne("Perfi.Core.Accounts.AccountAggregate.SummaryAccount", null)
                        .WithMany("Components")
                        .HasForeignKey("ParentAccountNumber")
                        .HasPrincipalKey("Number");
                });

            modelBuilder.Entity("Perfi.Core.Accounts.LoanAggregate.Loan", b =>
                {
                    b.OwnsOne("Perfi.Core.Accounting.Money", "LoanAmount", b1 =>
                        {
                            b1.Property<int>("LoanId")
                                .HasColumnType("integer");

                            b1.Property<string>("Currency")
                                .IsRequired()
                                .HasMaxLength(5)
                                .HasColumnType("character varying(5)");

                            b1.Property<decimal?>("Value")
                                .IsRequired()
                                .HasColumnType("numeric");

                            b1.HasKey("LoanId");

                            b1.ToTable("Loan");

                            b1.WithOwner()
                                .HasForeignKey("LoanId");
                        });

                    b.Navigation("LoanAmount")
                        .IsRequired();
                });

            modelBuilder.Entity("Perfi.Core.Expenses.Expense", b =>
                {
                    b.OwnsOne("Perfi.Core.Accounting.Money", "Amount", b1 =>
                        {
                            b1.Property<int>("ExpenseId")
                                .HasColumnType("integer");

                            b1.Property<string>("Currency")
                                .IsRequired()
                                .HasMaxLength(4)
                                .HasColumnType("character varying(4)");

                            b1.Property<decimal?>("Value")
                                .HasColumnType("numeric");

                            b1.HasKey("ExpenseId");

                            b1.ToTable("Expense");

                            b1.WithOwner()
                                .HasForeignKey("ExpenseId");
                        });

                    b.Navigation("Amount")
                        .IsRequired();
                });

            modelBuilder.Entity("Perfi.Core.Expenses.ExpensePaymentMethod", b =>
                {
                    b.HasOne("Perfi.Core.Expenses.Expense", null)
                        .WithOne("PaymentMethod")
                        .HasForeignKey("Perfi.Core.Expenses.ExpensePaymentMethod", "Id")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Perfi.Core.Expenses.TransactionalExpenseCategory", b =>
                {
                    b.HasOne("Perfi.Core.Expenses.SummaryExpenseCategory", null)
                        .WithMany("Categories")
                        .HasForeignKey("SummaryExpenseCategoryCode")
                        .HasPrincipalKey("Code")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Perfi.Core.Expenses.CashAccountExpensePaymentMethod", b =>
                {
                    b.HasOne("Perfi.Core.Expenses.ExpensePaymentMethod", null)
                        .WithOne()
                        .HasForeignKey("Perfi.Core.Expenses.CashAccountExpensePaymentMethod", "Id")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Perfi.Core.Expenses.CreditCardExpensePaymentMethod", b =>
                {
                    b.HasOne("Perfi.Core.Expenses.ExpensePaymentMethod", null)
                        .WithOne()
                        .HasForeignKey("Perfi.Core.Expenses.CreditCardExpensePaymentMethod", "Id")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Perfi.Core.Accounting.AccountingTransactionAggregate.AccountingTransaction", b =>
                {
                    b.Navigation("AccountingEntries");
                });

            modelBuilder.Entity("Perfi.Core.Expenses.Expense", b =>
                {
                    b.Navigation("PaymentMethod")
                        .IsRequired();
                });

            modelBuilder.Entity("Perfi.Core.Accounts.AccountAggregate.SummaryAccount", b =>
                {
                    b.Navigation("Components");
                });

            modelBuilder.Entity("Perfi.Core.Expenses.SummaryExpenseCategory", b =>
                {
                    b.Navigation("Categories");
                });
#pragma warning restore 612, 618
        }
    }
}
