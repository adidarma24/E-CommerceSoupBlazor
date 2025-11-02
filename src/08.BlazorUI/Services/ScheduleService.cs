using System.Net.Http.Json;
using System.Text.Json;
using MyApp.BlazorUI.DTOs;

namespace MyApp.BlazorUI.Services
{
    public class ScheduleService
    {
        private readonly HttpClient _httpClient;
        private readonly JsonSerializerOptions _jsonOptions = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };

        public ScheduleService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        // ========================================
        // 🔹 Helper untuk parsing pesan error JSON
        // ========================================
        private async Task<string> ExtractErrorMessageAsync(HttpResponseMessage response)
        {
            try
            {
                var content = await response.Content.ReadAsStringAsync();
                if (string.IsNullOrWhiteSpace(content))
                    return "Terjadi kesalahan pada server.";

                using var doc = JsonDocument.Parse(content);
                if (doc.RootElement.TryGetProperty("message", out var message))
                    return message.GetString() ?? "Terjadi kesalahan.";

                // Fallback: tampilkan ringkas
                return content.Length > 300 ? "Terjadi kesalahan di server." : content;
            }
            catch
            {
                return "Terjadi kesalahan di server.";
            }
        }

        // ========================================
        // ✅ GET SCHEDULES
        // ========================================
        public async Task<ApiResponse<List<ScheduleViewModel>>> GetSchedulesByCourseIdAsync(int courseId)
        {
            var response = await _httpClient.GetFromJsonAsync<ApiResponse<List<ScheduleViewModel>>>(
                $"api/menucourses/{courseId}/schedules"
            );

            return response ?? new ApiResponse<List<ScheduleViewModel>>
            {
                Success = false,
                Message = "Tidak ada data yang diterima dari server."
            };
        }

        // ========================================
        // ✅ DELETE SCHEDULE
        // ========================================
        public async Task<ApiResponse<bool>> DeleteScheduleAsync(int assignmentId, int scheduleId)
        {
            try
            {
                var relResponse = await _httpClient.DeleteAsync($"api/menucourses/schedules/{assignmentId}");
                if (!relResponse.IsSuccessStatusCode)
                {
                    var msg = await ExtractErrorMessageAsync(relResponse);
                    return new ApiResponse<bool> { Success = false, Message = msg };
                }

                // Hapus master schedule (non-fatal)
                var schedResponse = await _httpClient.DeleteAsync($"api/schedules/{scheduleId}");
                if (!schedResponse.IsSuccessStatusCode)
                {
                    var msg = await ExtractErrorMessageAsync(schedResponse);
                    Console.WriteLine($"⚠️ Gagal hapus master schedule: {msg}");
                }

                return new ApiResponse<bool>
                {
                    Success = true,
                    Message = "✅ Jadwal berhasil dihapus."
                };
            }
            catch (Exception ex)
            {
                return new ApiResponse<bool>
                {
                    Success = false,
                    Message = $"Gagal menghapus jadwal: {ex.Message}"
                };
            }
        }

        // ========================================
        // ✅ ADD SCHEDULE
        // ========================================
        public async Task<ApiResponse<bool>> AddScheduleAsync(int courseId, DateTimeOffset scheduleDate, int availableSlot)
        {
            try
            {
                // 1️⃣ Buat master schedule
                var createSchedulePayload = new { scheduleDate };
                var createScheduleResponse = await _httpClient.PostAsJsonAsync("/api/schedules", createSchedulePayload);

                if (!createScheduleResponse.IsSuccessStatusCode)
                {
                    var msg = await ExtractErrorMessageAsync(createScheduleResponse);
                    return new ApiResponse<bool> { Success = false, Message = msg };
                }

                var scheduleApiResponse = await createScheduleResponse.Content.ReadFromJsonAsync<ApiResponse<ScheduleViewModel>>(_jsonOptions);
                if (scheduleApiResponse?.Data == null)
                    return new ApiResponse<bool> { Success = false, Message = "Gagal membaca respons server." };

                int scheduleId = scheduleApiResponse.Data.Id;

                // 2️⃣ Assign ke course
                var assignDto = new CreateMenuCourseScheduleDto
                {
                    MenuCourseId = courseId,
                    ScheduleId = scheduleId,
                    AvailableSlot = availableSlot,
                    Status = "Active"
                };

                var assignResponse = await _httpClient.PostAsJsonAsync("/api/menucourses/schedules", assignDto);
                if (!assignResponse.IsSuccessStatusCode)
                {
                    var msg = await ExtractErrorMessageAsync(assignResponse);
                    return new ApiResponse<bool> { Success = false, Message = msg };
                }

                return new ApiResponse<bool> { Success = true, Message = "✅ Jadwal berhasil ditambahkan." };
            }
            catch (Exception ex)
            {
                return new ApiResponse<bool> { Success = false, Message = $"Exception: {ex.Message}" };
            }
        }

        // ========================================
        // ✅ UPDATE RELATION (SLOT & STATUS)
        // ========================================
        public async Task<ApiResponse<bool>> UpdateAssignmentAsync(int scheduleAssignmentId, UpdateMenuCourseScheduleDto model)
        {
            try
            {
                var response = await _httpClient.PutAsJsonAsync($"api/menucourses/schedules/{scheduleAssignmentId}", model);
                if (!response.IsSuccessStatusCode)
                {
                    var msg = await ExtractErrorMessageAsync(response);
                    return new ApiResponse<bool> { Success = false, Message = msg };
                }

                return new ApiResponse<bool> { Success = true, Message = "✅ Jadwal berhasil diperbarui." };
            }
            catch (Exception ex)
            {
                return new ApiResponse<bool> { Success = false, Message = $"Exception: {ex.Message}" };
            }
        }

        // ========================================
        // ✅ UPDATE SCHEDULE (DATE, SLOT, STATUS)
        // ========================================
        public async Task<ApiResponse<bool>> UpdateScheduleAsync(UpdateMenuCourseScheduleDto model)
        {
            try
            {
                // 1️⃣ Update tanggal master schedule (jika ada)
                if (model.ScheduleId > 0 && model.ScheduleDate.HasValue)
                {
                    var updateSchedulePayload = new { scheduleDate = model.ScheduleDate.Value };
                    var scheduleUpdateResponse = await _httpClient.PutAsJsonAsync(
                        $"api/schedules/{model.ScheduleId}", updateSchedulePayload
                    );

                    if (!scheduleUpdateResponse.IsSuccessStatusCode)
                    {
                        var msg = await ExtractErrorMessageAsync(scheduleUpdateResponse);
                        return new ApiResponse<bool> { Success = false, Message = msg };
                    }
                }

                // 2️⃣ Update relasi jadwal (slot & status)
                var updateAssignmentPayload = new
                {
                    availableSlot = model.AvailableSlot,
                    status = model.Status
                };

                var assignmentUpdateResponse = await _httpClient.PutAsJsonAsync(
                    $"api/menucourses/schedules/{model.Id}", updateAssignmentPayload
                );

                if (!assignmentUpdateResponse.IsSuccessStatusCode)
                {
                    var msg = await ExtractErrorMessageAsync(assignmentUpdateResponse);
                    return new ApiResponse<bool> { Success = false, Message = msg };
                }

                return new ApiResponse<bool> { Success = true, Message = "✅ Jadwal berhasil diperbarui." };
            }
            catch (Exception ex)
            {
                return new ApiResponse<bool> { Success = false, Message = $"Exception: {ex.Message}" };
            }
        }
    }
}
