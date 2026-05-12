namespace green_city_sh.Tests.Api.DTO.Habits;

public class HabitFavorites
{
    public List<HabitPageItem> Page { get; set; } = new();
    public int TotalElements { get; set; }
    public int CurrentPage { get; set; }
    public int TotalPages { get; set; }
}

public class HabitPageItem
{
    public long Id { get; set; }
    public int DefaultDuration { get; set; }
    public int Complexity { get; set; }
    public string Image { get; set; } = string.Empty;
    public bool IsCustomHabit { get; set; }
    public bool IsFavorite { get; set; }
    public HabitTranslationResponse HabitTranslation { get; set; } = new();
}

public class HabitTranslationResponse
{
    public string Name { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public string HabitItem { get; set; } = string.Empty;
    public string LanguageCode { get; set; } = string.Empty;
}