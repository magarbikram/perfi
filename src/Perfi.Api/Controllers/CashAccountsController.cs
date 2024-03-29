﻿using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Perfi.Api.Commands;
using Perfi.Api.Responses;
using Perfi.Api.Services;
using Perfi.Core.Expenses;

namespace Perfi.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CashAccountsController : ControllerBase
    {
        private readonly IAddCashAccountService _addCashAccountService;
        private readonly ICashAccountQueryService _cashAccountQueryService;

        public CashAccountsController(
            IAddCashAccountService addCashAccountService,
            ICashAccountQueryService cashAccountQueryService)
        {
            _addCashAccountService = addCashAccountService;
            _cashAccountQueryService = cashAccountQueryService;
        }
        [HttpPost]
        public async Task<ActionResult<NewCashAccountAddedResponse>> AddAsync([FromBody] AddNewCashAccountCommand addNewCashAccountCommand)
        {
            NewCashAccountAddedResponse newCashAccountAddedResponse = await _addCashAccountService.AddAsync(addNewCashAccountCommand);
            return Created("", newCashAccountAddedResponse);
        }

        [HttpGet]
        public async Task<ActionResult<List<ListCashAccountResponse>>> AllAsync(bool withCurrentBalance)
        {
            List<ListCashAccountResponse> listCashAccountResponses = await _cashAccountQueryService.GetAllAsync(withCurrentBalance);
            return Ok(listCashAccountResponses);
        }


        [HttpGet("{cashAccountId}/Transactions/CurrentPeriod")]
        public async Task<ActionResult<List<TransactionResponse>>> GetTransactionsAsync(int cashAccountId)
        {
            List<TransactionResponse> transactions = await _cashAccountQueryService.GetAllTransactionsAsync(cashAccountId, TransactionPeriod.CurrentPeriod());
            return Ok(transactions);
        }
    }
}
