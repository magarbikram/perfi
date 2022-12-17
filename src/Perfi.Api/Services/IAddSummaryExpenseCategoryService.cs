using Perfi.Api.Commands;
using Perfi.Api.Responses;

namespace Perfi.Api.Services
{
    public interface IAddSummaryExpenseCategoryService
    {
        Task<NewSummaryExpenseCategoryAddedResponse> AddAsync(AddNewSummaryExpenseCategoryCommand addNewSummaryExpenseCategoryCommand);
    }
}