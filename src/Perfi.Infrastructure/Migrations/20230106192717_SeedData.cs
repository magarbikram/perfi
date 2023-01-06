using Microsoft.EntityFrameworkCore.Migrations;
using Perfi.Core.Accounts.AccountAggregate;
using Perfi.Core.Expenses;

#nullable disable

namespace Perfi.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class SeedData : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@$"
INSERT INTO public.""Account""(
	                ""AccountCategory"", ""Number"", ""Name"", ""Type"")
	         VALUES ('{AccountCategory.Assets}', '{SummaryAccount.DefaultAccountNumbers.BankCashAccount}', 'Bank Cash Accounts', 'Summary');
                
                INSERT INTO public.""Account""(
	                ""AccountCategory"", ""Number"", ""Name"", ""Type"")
	         VALUES ('{AccountCategory.Liabilities}', '{SummaryAccount.DefaultAccountNumbers.CreditCardAccount}', 'Credit Card - Payable', 'Summary');
INSERT INTO public.""Account""(
	                ""AccountCategory"", ""Number"", ""Name"", ""Type"")
	         VALUES ('{AccountCategory.Liabilities}', '{SummaryAccount.DefaultAccountNumbers.LoanAccount}', 'Loan - Payable', 'Summary');
INSERT INTO public.""Account""(
	                ""AccountCategory"", ""Number"", ""Name"", ""Type"")
	         VALUES ('{AccountCategory.Expenses}', '{SummaryAccount.DefaultAccountNumbers.HomeExpensesAccount}', 'Home Expenses', 'Summary');
INSERT INTO public.""Account""(
	                ""AccountCategory"", ""Number"", ""Name"", ""Type"")
	         VALUES ('{AccountCategory.Equity}', '{SummaryAccount.DefaultAccountNumbers.EquityAccount}', 'Retained Earnings', 'Summary');

INSERT INTO public.""Account""(
	                ""AccountCategory"", ""Number"", ""Name"", ""Type"", ""ParentAccountNumber"")
	         VALUES ('{AccountCategory.Equity}', '{TransactionalAccount.DefaultAccountNumbers.InterestPaid}', 'Interest Paid', 'Transactional', '{SummaryAccount.DefaultAccountNumbers.EquityAccount}');
INSERT INTO public.""Account""(
	                ""AccountCategory"", ""Number"", ""Name"", ""Type"", ""ParentAccountNumber"")
	         VALUES ('{AccountCategory.Equity}', '{TransactionalAccount.DefaultAccountNumbers.EscrowPaid}', 'Escrow Paid', 'Transactional', '{SummaryAccount.DefaultAccountNumbers.EquityAccount}');
INSERT INTO public.""Account""(
	                ""AccountCategory"", ""Number"", ""Name"", ""Type"", ""ParentAccountNumber"")
	         VALUES ('{AccountCategory.Equity}', '{TransactionalAccount.DefaultAccountNumbers.FeePaid}', 'Fee Paid', 'Transactional', '{SummaryAccount.DefaultAccountNumbers.EquityAccount}');

INSERT INTO public.""ExpenseCategory""(
	 ""Code"", ""Name"", ""Type"")
	VALUES ('{ExpenseCategoryCode.Housing.Value}', 'Housing', 'Summary');
INSERT INTO public.""ExpenseCategory""(
	 ""Code"", ""Name"", ""Type"", ""SummaryExpenseCategoryCode"")
	VALUES ('{ExpenseCategoryCode.MortgagePayment.Value}', 'Mortgage', 'Transactional', '{ExpenseCategoryCode.Housing.Value}');

INSERT INTO public.""ExpenseCategory""(
	 ""Code"", ""Name"", ""Type"")
	VALUES ('{ExpenseCategoryCode.Debts.Value}', 'Debts', 'Summary');
INSERT INTO public.""ExpenseCategory""(
	 ""Code"", ""Name"", ""Type"", ""SummaryExpenseCategoryCode"")
	VALUES ('{ExpenseCategoryCode.DebtPayment.Value}', 'Debt', 'Transactional', '{ExpenseCategoryCode.Debts.Value}');

            ");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {

        }
    }
}
