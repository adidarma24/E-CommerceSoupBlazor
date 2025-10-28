using Microsoft.AspNetCore.Components.Forms;

namespace MyApp.BlazorUI.Models
{

    public class CreateCategoryDto
    {
        public string Name { get; set; } = string.Empty;
        public IBrowserFile? ImageFile { get; set; } // diubah dari string ke file
        public string? Description { get; set; }
    }

    public class UpdateCategoryDto
    {
        public string? Name { get; set; }
        public IBrowserFile? ImageFile { get; set; }
        public string? Description { get; set; }
    }

    public class ApiResponse<T>
    {
        public bool Success { get; set; }
        public T? Data { get; set; }
        public string? Message { get; set; }
    }
}
