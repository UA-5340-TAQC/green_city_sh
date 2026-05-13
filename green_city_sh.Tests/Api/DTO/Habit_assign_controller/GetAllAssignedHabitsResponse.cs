namespace green_city_sh.Tests.Api.DTO.Habit_assign_controller
{
    public class GetAllAssignedHabitsResponse
    {
        public string CreateDateTime { get; set; } = string.Empty;
        public int Duration { get; set; }
        public int Id { get; set; }
        public string LastEnrollmentDate { get; set; } = string.Empty;
        public string Status { get; set; } = string.Empty;
        public int UserId { get; set; }
        public int WorkingDays { get; set; }
    }

    public class HabitActivityResponse
    {
        public string EnrollDate { get; set; } = string.Empty;
        public List<HabitAssignActivity> HabitAssigns { get; set; } = new();
    }

    public class HabitAssignActivity
    {
        public int HabitAssignId { get; set; }
        public string HabitName { get; set; } = string.Empty;
        public string HabitDescription { get; set; } = string.Empty;
        public bool Enrolled { get; set; }
    }
}