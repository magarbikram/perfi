using Microsoft.EntityFrameworkCore;
using Perfi.Api.Responses;
using Perfi.Api.Responses.Mappers;
using Perfi.Api.Services;
using Perfi.Core.Accounts.AccountAggregate;
using Perfi.Core.Accounts.AccountingTransactionAggregate;
using Perfi.Core.Accounts.CashAccountAggregate;
using Perfi.Core.Accounts.CreditCardAggregate;
using Perfi.Core.Accounts.LoanAggregate;
using Perfi.Core.Earnings;
using Perfi.Core.Earnings.IncomeSources;
using Perfi.Core.Expenses;
using Perfi.Core.MoneyTransfers;
using Perfi.Core.Payments.IncomingPayments;
using Perfi.Core.Payments.LoanPayments;
using Perfi.Core.Payments.OutgoingPayments;
using Perfi.Core.SplitPartners;
using Perfi.Infrastructure.Database;
using Perfi.Infrastructure.Database.Repositories;
using System.Text.Json.Serialization;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(
        policy =>
        {
            policy.AllowAnyOrigin().AllowAnyHeader().AllowAnyMethod();
        });
});
// Add services to the container.
string? connectionString = builder.Configuration.GetConnectionString("DatabaseConnectionString");  //Configuration.GetConnectionString("DefaultConnection");

builder.Services.AddDbContext<AppDbContext>(options =>
          options.UseNpgsql(connectionString));

builder.Services.AddControllers().AddJsonOptions(options =>
{
    options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
});
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();


//repos
builder.Services.AddScoped<ICashAccountRepository, CashAccountRepository>();
builder.Services.AddScoped<ISummaryAccountRepository, SummaryAccountRepository>();
builder.Services.AddScoped<ITransactionalAccountRepository, TransactionalAccountRepository>();
builder.Services.AddScoped<ICreditCardAccountRepository, CreditCardAccountRepository>();
builder.Services.AddScoped<ILoanRepository, LoanRepository>();
builder.Services.AddScoped<ISummaryExpenseCategoryRepository, SummaryExpenseCategoryRepository>();
builder.Services.AddScoped<ITransactionalExpenseCategoryRepository, TransactionalExpenseCategoryRepository>();
builder.Services.AddScoped<IAccountingTransactionRepository, AccountingTransactionRepository>();
builder.Services.AddScoped<IExpenseRepository, ExpenseRepository>();
builder.Services.AddScoped<IIncomeSourceRepository, IncomeSourceRepository>();
builder.Services.AddScoped<IIncomeDocumentRepository, IncomeDocumentRepository>();
builder.Services.AddScoped<ISplitPartnerRepository, SplitPartnerRepository>();
builder.Services.AddScoped<IMoneyTransferRepository, MoneyTransferRepository>();
builder.Services.AddScoped<IIncomingPaymentRepository, IncomingPaymentRepository>();
builder.Services.AddScoped<IOutgoingPaymentRepository, OutgoingPaymentRepository>();
builder.Services.AddScoped<ILoanPaymentRepository, LoanPaymentRepository>();

//services
builder.Services.AddScoped<IAddCashAccountService, AddCashAccountService>();
builder.Services.AddScoped<ICashAccountQueryService, CashAccountQueryService>();
builder.Services.AddScoped<IAddCreditCardAccountService, AddCreditCardAccountService>();
builder.Services.AddScoped<ICreditCardAccountQueryService, CreditCardAccountQueryService>();
builder.Services.AddScoped<IAddLoanService, AddLoanService>();
builder.Services.AddScoped<ILoanQueryService, LoanQueryService>();
builder.Services.AddScoped<IAddSummaryExpenseCategoryService, AddSummaryExpenseCategoryService>();
builder.Services.AddScoped<ISummaryExpenseCategoryQueryService, SummaryExpenseCategoryQueryService>();
builder.Services.AddScoped<IAddTransactionalExpenseCategoryService, AddTransactionalExpenseCategoryService>();
builder.Services.AddScoped<IAddExpenseAccountService, AddExpenseAccountService>();
builder.Services.AddScoped<IExpenseAccountQueryService, ExpenseAccountQueryService>();
builder.Services.AddScoped<IAddNewExpenseService, AddNewExpenseService>();
builder.Services.AddScoped<IExpenseQueryService, ExpenseQueryService>();
builder.Services.AddScoped<IGetNextAccountNumberService, GetNextAccountNumberService>();
builder.Services.AddScoped<IAddNewIncomeSourceService, AddNewIncomeSourceService>();
builder.Services.AddScoped<IIncomeSourceQueryService, IncomeSourceQueryService>();
builder.Services.AddScoped<IAddNewIncomeService, AddNewIncomeService>();
builder.Services.AddScoped<IIncomeDocumentQueryService, IncomeDocumentQueryService>();
builder.Services.AddScoped<ICashFlowReportService, CashFlowReportService>();
builder.Services.AddScoped<IAddOutgoingPaymentService, AddOutgoingPaymentService>();

builder.Services.AddScoped<IPayCreditCardService, PayCreditCardService>();
builder.Services.AddScoped<IPayLoanService, PayLoanService>();
builder.Services.AddScoped<ICalculateCurrentBalanceService, CalculateCurrentBalanceService>();
builder.Services.AddScoped<IBuildSummaryAccountResponseService, BuildSummaryAccountResponseService>();
builder.Services.AddScoped<IAddSplitPartnerService, AddSplitPartnerService>();
builder.Services.AddScoped<ISplitPartnerQueryService, SplitPartnerQueryService>();
builder.Services.AddScoped<ITransferMoneyService, TransferMoneyService>();
builder.Services.AddScoped<IMoneyTransferQueryService, MoneyTransferQueryService>();
builder.Services.AddScoped<ListExpenseResponseMapper>();
var app = builder.Build();
app.UseCors();
// Configure the HTTP request pipeline.
//if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
