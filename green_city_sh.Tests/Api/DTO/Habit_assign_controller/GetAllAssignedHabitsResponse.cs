// DTO/Habit_assign_controller/GetAllAssignedHabitsResponse.cs

namespace green_city_sh.Tests.Api.DTO.Habit_assign_controller
{
    /// <summary>
    /// Represents a response containing assigned habit information
    /// </summary>
    public class GetAllAssignedHabitsResponse
    {
        /// <summary>Date and time when the habit assignment was created</summary>
        public string CreateDateTime { get; set; } = string.Empty;
        /// <summary>Duration of the habit in days</summary>
        public int Duration { get; set; }
        /// <summary>Unique identifier of the habit assignment</summary>
        public int Id { get; set; }
        /// <summary>Date and time of the last enrollment</summary>
        public string LastEnrollmentDate { get; set; } = string.Empty;
        /// <summary>Current status of the habit (INPROGRESS or ACQUIRED)</summary>
        public string Status { get; set; } = string.Empty;
        /// <summary>Unique identifier of the user</summary>
        public int UserId { get; set; }
        /// <summary>Number of days the habit was worked on</summary>
        public int WorkingDays { get; set; }
    }

    /// <summary>
    /// Represents user activities for a specific date
    /// </summary>
    public class HabitActivityResponse
    {
        /// <summary>Date of the activities</summary>
        public string EnrollDate { get; set; } = string.Empty;
        /// <summary>List of habit assignments for this date</summary>
        public List<HabitAssignActivity> HabitAssigns { get; set; } = new();
    }

    /// <summary>
    /// Represents a single habit assignment activity
    /// </summary>
    public class HabitAssignActivity
    {
        /// <summary>Unique identifier of the habit assignment</summary>
        public int HabitAssignId { get; set; }
        /// <summary>Name of the habit</summary>
        public string HabitName { get; set; } = string.Empty;
        /// <summary>Description of the habit</summary>
        public string HabitDescription { get; set; } = string.Empty;
        /// <summary>Indicates whether the habit was enrolled on this date</summary>
        public bool Enrolled { get; set; }
    }
}
