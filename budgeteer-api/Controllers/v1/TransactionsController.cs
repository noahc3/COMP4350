using BudgeteerAPI.Models;
using BudgeteerAPI.Models.Requests;
using BudgeteerAPI.Services;
using Microsoft.AspNetCore.Mvc;

namespace BudgeteerAPI.Controllers.v1
{
    [ApiController]
    [Route("v1/transactions")]
    public class TransactionsController : ControllerBase
    {
        [HttpPost("import")]
        public IActionResult ImportTransactions([FromBody] ImportTransactionsRequest request) {
            if (request.data is null) return BadRequest("Import data is missing.");
            Transaction[] result = TransactionsService.ImportTransactions(request.source, request.data);
            return Ok(result);
        }
    }
}
