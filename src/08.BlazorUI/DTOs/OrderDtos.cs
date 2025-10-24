// File: DTOs/OrderDtos.cs
namespace MyApp.BlazorUI.DTOs
{
    public record CreateOrderDto(List<int> ScheduleIds, decimal TotalPrice, int PaymentMethodId);

    public record PaymentResponseDto(string OrderId, string PaymentToken, string PaymentUrl);

    public record ErrorResponseDto(string Message, List<string>? Errors);
}