namespace MyApp.WebApi.DTOs // Sesuaikan jika nama proyek Anda bukan MyApp.WebApi
{
    // DTO yang diterima dari Blazor
    public record CreateOrderDto(List<int> ScheduleIds, decimal TotalPrice, int PaymentMethodId);

    // DTO yang dikirim kembali ke Blazor
    public record PaymentResponseDto(string OrderId, string PaymentToken, string PaymentUrl);
}