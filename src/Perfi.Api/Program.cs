using Microsoft.EntityFrameworkCore;
using Perfi.Api.Responses;
using Perfi.Api.Services;
using Perfi.Core.Accounts.AccountAggregate;
using Perfi.Core.Accounts.AccountingTransactionAggregate;
using Perfi.Core.Accounts.CashAccountAggregate;
using Perfi.Core.Accounts.CreditCardAggregate;
using Perfi.Core.Accounts.LoanAggregate;
using Perfi.Core.Expenses;
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

var app = builder.Build();
app.UseCors();
// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
