using Microsoft.AspNetCore.Mvc;
using project_web1.Models;
using project_web1.Service;
namespace project_web1.Controllers
{
    public class PaymentController : Controller
    {
        private readonly IMomoService _momoService;

        public PaymentController(IMomoService momoService)
        {
            _momoService = momoService;
        }

        [HttpPost]
        public async Task<IActionResult> CreatePayment([FromBody] OrderInfo courseInfo)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var orderInfo = new OrderInfo
            {
                OrderInformation = courseInfo.OrderInformation,
                Amount = 100000 // Set your amount here
            };

            var result = await _momoService.CreatePaymentAsync(orderInfo);
            return Ok(result);
        }

        [HttpGet("callback")]
        public IActionResult PaymentCallback([FromQuery] string orderId, [FromQuery] string resultCode)
        {
            // Handle the payment callback here
            // You should verify the payment status with Momo's API
            if (resultCode == "0")
            {
                return Ok(new { Message = "Payment successful", OrderId = orderId });
            }
            else
            {
                return BadRequest(new { Message = "Payment failed", OrderId = orderId });
            }
        }
    }
}
