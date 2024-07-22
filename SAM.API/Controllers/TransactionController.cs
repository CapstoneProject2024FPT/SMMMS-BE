using DentalLabManagement.API.Controllers;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SAM.BusinessTier.Constants;
using SAM.BusinessTier.Payload.City;
using SAM.BusinessTier.Payload.Transaction;
using SAM.BusinessTier.Services.Implements;
using SAM.BusinessTier.Services.Interfaces;

namespace SAM.API.Controllers
{
    [ApiController]
    public class TransactionController : BaseController<TransactionController>
    {
        readonly ITransactionService _transactionService;

        public TransactionController(ILogger<TransactionController> logger, ITransactionService transactionService) : base(logger)
        {
            _transactionService = transactionService;
        }
        [HttpGet(ApiEndPointConstant.Transaction.TransactionEndPoint)]
        public async Task<IActionResult> GetTransactionById(Guid id)
        {
            var response = await _transactionService.GetTransactionById(id);
            return Ok(response);
        }
        [HttpGet(ApiEndPointConstant.Transaction.TransactionsEndPoint)]
        public async Task<IActionResult> GetTransactionList([FromQuery]TransactionFilter filter)
        {
            var response = await _transactionService.GetTransactionList(filter);
            return Ok(response);
        }
    }
}
