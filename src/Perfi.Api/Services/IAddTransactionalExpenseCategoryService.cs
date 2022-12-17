using Perfi.Api.Commands;
using Perfi.Api.Responses;

namespace Perfi.Api.Services
{
    public interface IAddTransactionalExpenseCategoryService
    {
        Task<NewTransactionalExpenseCategoryAddedResponse> AddAsync(AddNewTransactionalExpenseCategoryCommand addNewTransactionalExpenseCategoryCommand);
    }
}