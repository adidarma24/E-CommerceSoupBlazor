namespace MyApp.BlazorUI.DTOs
{
    // Digunakan untuk menampilkan data di UI (hasil GET dari API gabungan Schedule + MenuCourseSchedule)
    public class ScheduleViewModel
    {
        public int Id { get; set; }
        public int ScheduleId { get; set; }
        public int MenuCourseId { get; set; }
        public string MenuCourseName { get; set; } = string.Empty;
        public DateTimeOffset ScheduleDate { get; set; }
        public int AvailableSlot { get; set; }
        public string Status { get; set; } = "Active";
    }

    public class CreateMenuCourseScheduleDto
    {
        public int MenuCourseId { get; set; }
        public int ScheduleId { get; set; }
        public int AvailableSlot { get; set; }
        public string Status { get; set; } = "Active";
    }


    public class UpdateMenuCourseScheduleDto
    {
        public int Id { get; set; }
        public int? ScheduleId { get; set; }
        public int MenuCourseId { get; set; }
        public DateTimeOffset? ScheduleDate { get; set; }

        public int AvailableSlot { get; set; }
        public string Status { get; set; } = "Active";
    }
}
