using Microsoft.AspNetCore.Mvc;
// Tambahkan using statement untuk DTO Anda
using MyApp.WebApi.DTOs;

[ApiController]
[Route("api/[controller]")] // Ini akan menjadi "api/orders"
public class OrdersController : ControllerBase
{
    [HttpPost("create")]
    public async Task<IActionResult> CreateOrder([FromBody] CreateOrderDto createOrderRequest)
    {
        try
        {
            var newOrderId = "ORD-" + Guid.NewGuid().ToString().Substring(0, 8).ToUpper();
            var paymentResponse = new PaymentResponseDto(
                OrderId: newOrderId,
                PaymentToken: "dummy-token-from-backend", // Token jika ada
                PaymentUrl: "" // URL pembayaran jika ada
            );
            
            // Mengembalikan 200 OK beserta datanya
            return Ok(paymentResponse);
        }
        catch (Exception ex)
        {
            // Jika terjadi error
            return StatusCode(500, $"Terjadi kesalahan internal: {ex.Message}");
        }
    }
}