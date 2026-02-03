using Microsoft.AspNetCore.Mvc;
using Razorpay.Api;
using PaymentService.DTOs;

namespace PaymentService.Controllers;

[ApiController]
[Route("api/payments")]
public class PaymentsController : ControllerBase
{
  private readonly string _keyId;
  private readonly string _keySecret;

  public PaymentsController(IConfiguration config)
  {
    _keyId = config["Razorpay:KeyId"]!;
    _keySecret = config["Razorpay:KeySecret"]!;
  }

  // ================= CREATE ORDER =================
  [HttpPost("create-order")]
  public IActionResult CreateOrder([FromBody] OrderRequest req)
  {
    try
    {
      RazorpayClient client = new RazorpayClient(_keyId, _keySecret);

      Dictionary<string, object> options = new()
            {
                { "amount", req.amount },
                { "currency", req.currency },
                { "receipt", $"txn_{Guid.NewGuid()}" }
            };

      Order order = client.Order.Create(options);

      return Ok(new OrderResponse(
          order["id"].ToString(),
          req.amount,
          req.currency
      ));
    }
    catch (Exception ex)
    {
      return StatusCode(500, ex.Message);
    }
  }

  // ================= VERIFY PAYMENT =================
  [HttpPost("verify")]
  public IActionResult VerifyPayment([FromBody] PaymentVerificationRequest req)
  {
    try
    {
      Dictionary<string, string> attributes = new()
        {
            { "razorpay_order_id", req.razorpayOrderId },
            { "razorpay_payment_id", req.razorpayPaymentId },
            { "razorpay_signature", req.razorpaySignature }
        };

      Utils.verifyPaymentSignature(attributes);

      // 🔔 SUCCESS – update DB in caller service if needed
      return Ok(new
      {
        status = "success",
        message = "Payment Verified"
      });
    }
    catch (Exception ex)
    {
      return StatusCode(500, $"Verification error: {ex.Message}");
    }
  }
}
