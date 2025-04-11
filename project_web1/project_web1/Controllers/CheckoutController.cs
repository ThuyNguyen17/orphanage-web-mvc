using Microsoft.AspNetCore.Mvc;
using project_web1.Models;
using project_web1.Service;
using System.Security.Claims;
using System.Threading.Tasks;

namespace project_web1.Controllers
{
    public class CheckoutController : Controller
    {
        private readonly TrungTamTreContext _dataContext; // Inject DataContext
        private readonly IMomoService _momoService; // Inject Momo Service

        // Constructor để inject DataContext và MomoService vào controller
        public CheckoutController(TrungTamTreContext dataContext, IMomoService momoService)
        {
            _dataContext = dataContext;
            _momoService = momoService; // Inject MomoService
        }

        [HttpGet]
        public async Task<IActionResult> PaymentCallBack()
        {
            var requestQuery = HttpContext.Request.Query;

            if (requestQuery["resultCode"] == "0") // Kiểm tra giao dịch thành công
            {
                // Lưu thông tin giao dịch vào cơ sở dữ liệu
                var newMomoInsert = new OrderInfo
                {
                    OrderId = requestQuery["orderId"],
                    FullName = User.FindFirstValue(ClaimTypes.Email),
                    Amount = decimal.TryParse(requestQuery["Amount"], out var amount) ? amount : 0,
                    OrderInformation = requestQuery["orderInfo"],
                };

                _dataContext.Add(newMomoInsert);
                await _dataContext.SaveChangesAsync();

                // Cập nhật trạng thái thanh toán thành công vào cơ sở dữ liệu
                var checkoutResult = await CheckoutAsync(requestQuery["orderId"]);
                return View("PaymentSuccess", checkoutResult); // Trả kết quả cho View PaymentSuccess
            }
            else
            {
                TempData["error"] = "Momo transaction canceled or failed.";
                return RedirectToAction("Index", "Home");
            }
        }

        private async Task<string> CheckoutAsync(string orderId)
        {
           
            return $"Order ID: {orderId} not found.";
        }
    }
}
