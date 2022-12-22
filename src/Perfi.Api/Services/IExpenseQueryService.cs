﻿using Perfi.Api.Responses;

namespace Perfi.Api.Services
{
    public interface IExpenseQueryService
    {
        Task<IEnumerable<ListExpenseResponse>> GetCurrentExpensesAsync();
        Task<IEnumerable<ListExpenseResponse>> GetCurrentTop10ExpensesAsync();
    }
}