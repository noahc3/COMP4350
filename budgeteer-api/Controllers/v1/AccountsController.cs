using BudgeteerAPI.Extensions;
using BudgeteerAPI.Middleware;
using BudgeteerAPI.Models;
using BudgeteerAPI.Models.Requests;
using BudgeteerAPI.Services;
using Microsoft.AspNetCore.Mvc;
using System.Runtime.InteropServices;
using System.Transactions;

namespace BudgeteerAPI.Controllers.v1
{
    [ApiController]
    [Route("v1/account")]
    public class AccountsController : ControllerBase
    {
        [HttpGet]
        [AuthenticationRequired]
        public async Task<IActionResult> GetAccounts(AccountService service)
        {
            var user = HttpContext.GetUserProfile();
            List<Account> accounts = await service.GetUserAccountsAsync(user!.Id);
            return Ok(accounts);
        }

        [HttpPost]
        [AuthenticationRequired]
        public async Task<IActionResult> AddAccount([FromBody] Account account, [FromServices] AccountService service)
        {
            var user = HttpContext.GetUserProfile();
            Account result = await service.AddAccountAsync(user!.User, account);
            return Ok(result);
        }

        [HttpPut]
        [AuthenticationRequired]
        public async Task<IActionResult> UpdateAccount([FromBody] Account account, [FromServices] AccountService service)
        {
            var user = HttpContext.GetUserProfile();
            try
            {
                await service.UpdateAccountAsync(user!.User, account);
            } catch (TransactionException ex)
            {
                return BadRequest(ex.Message);
            }

            return NoContent();
        }

        [HttpDelete("{accountId}")]
        [AuthenticationRequired]
        public async Task<IActionResult> DeleteAccount([FromRoute] string accountId, [FromServices] AccountService service)
        {
            var user = HttpContext.GetUserProfile();
            try
            {
                await service.DeleteAccountAsync(user!.User, accountId);
            } catch (TransactionException ex)
            {
                return BadRequest(ex.Message);
            }

            return NoContent();
        }
    }
}
