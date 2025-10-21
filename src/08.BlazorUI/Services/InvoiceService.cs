using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text.Json;
using MyApp.BlazorUI.Models;

namespace MyApp.BlazorUI.Services
{
    public class InvoiceService
    {
        private readonly HttpClient _httpClient;

        public InvoiceService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        // Ambil semua invoice — optional token parameter
        public async Task<List<InvoiceItem>> GetInvoicesAsync(string? token = null)
        {
            try
            {
                using var request = new HttpRequestMessage(HttpMethod.Get, "api/Invoices?pageNumber=1&pageSize=10");
                if (!string.IsNullOrWhiteSpace(token))
                    request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);

                var response = await _httpClient.SendAsync(request);
                response.EnsureSuccessStatusCode();

                // Debug JSON
                var json = await response.Content.ReadAsStringAsync();
                Console.WriteLine($"🧾 Invoice JSON Response: {json}");

                // Deserialize menggunakan model InvoiceResponse → Data → Items
                var wrapped = System.Text.Json.JsonSerializer.Deserialize<InvoiceResponse>(
                    json,
                    new System.Text.Json.JsonSerializerOptions
                    {
                        PropertyNameCaseInsensitive = true
                    }
                );

                return wrapped?.Data?.Items ?? new List<InvoiceItem>();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"❌ Error fetching invoices: {ex.Message}");
                return new List<InvoiceItem>();
            }
        }


        // Ambil detail invoice — optional token parameter
        public async Task<InvoiceDetailModel?> GetInvoiceDetailAsync(int invoiceId, string? token = null)
        {
            try
            {
                using var request = new HttpRequestMessage(HttpMethod.Get, $"api/Invoices/{invoiceId}");

                if (!string.IsNullOrWhiteSpace(token))
                    request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);

                var response = await _httpClient.SendAsync(request);
                response.EnsureSuccessStatusCode();

                var json = await response.Content.ReadAsStringAsync();
                Console.WriteLine($"🧾 Invoice Detail JSON Response: {json}");

                var result = JsonSerializer.Deserialize<ApiResponse<InvoiceDetailModel>>(
                    json,
                    new JsonSerializerOptions { PropertyNameCaseInsensitive = true }
                );

                return result?.Data;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"❌ Error fetching invoice detail: {ex.Message}");
                return null;
            }
        }
    }
}
