using Microsoft.EntityFrameworkCore;
using Perfi.Api.Responses;
using Perfi.Api.Services;
using Perfi.Core.Accounts.AccountAggregate;
using Perfi.Core.Accounts.CashAccountAggregate;
using Perfi.Core.Accounts.CreditCardAggregate;
using Perfi.Core.Accounts.LoanAggregate;
using Perfi.Infrastructure.Database;
using Perfi.Infrastructure.Database.Repositories;

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

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();


//repos
builder.Services.AddScoped<ICashAccountRepository, CashAccountRepository>();
builder.Services.AddScoped<ISummaryAccountRepository, SummaryAccountRepository>();
builder.Services.AddScoped<ITransactionalAccountRepository, TransactionalAccountRepository>();
builder.Services.AddScoped<ICreditCardAccountRepository, CreditCardAccountRepository>();
builder.Services.AddScoped<ILoanRepository, LoanRepository>();
//services
builder.Services.AddScoped<IAddCashAccountService, AddCashAccountService>();
builder.Services.AddScoped<ICashAccountQueryService, CashAccountQueryService>();
builder.Services.AddScoped<IAddCreditCardAccountService, AddCreditCardAccountService>();
builder.Services.AddScoped<ICreditCardAccountQueryService, CreditCardAccountQueryService>();
builder.Services.AddScoped<IAddLoanService, AddLoanService>();
builder.Services.AddScoped<ILoanQueryService, LoanQueryService>();


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
