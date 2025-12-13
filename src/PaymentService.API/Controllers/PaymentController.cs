using Common.Extensions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PaymentService.Domain.Interfaces;

namespace PaymentService.API.Controllers
{
    [Route("api/payment")]
    [ApiController]
    public class PaymentController : ControllerBase
    {
        private readonly IAuthClientForPayment authClient;
        public PaymentController(IAuthClientForPayment authClient)
        {
            this.authClient = authClient;
        }

        [HttpPost("balance/deposit")]
        public async Task<IActionResult> Deposit([FromBody] decimal amount)
        {
            try
            {
                var userId = User.GetUserId();

                await authClient.DepositMoney(userId, (double)amount);

                return Ok($"Вы успешно пополнили счет на {amount}$!");
            }
            catch (Exception ex)
            {
                return BadRequest($"Error: {ex}");
            }
        }
    }
}
