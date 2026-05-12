namespace green_city_sh.Tests.Api.DTO.Habits;

public class HabitFavorites
{
    public List<HabitPageItem> Page { get; set; }
    public int TotalElements { get; set; }
    public int CurrentPage { get; set; }
    public int TotalPages { get; set; }
}

public class HabitPageItem
{
    public long Id { get; set; }
    public int DefaultDuration { get; set; }
    public int Complexity { get; set; }
    public string Image { get; set; }
    public bool IsCustomHabit { get; set; }
    public bool IsFavorite { get; set; }
    public HabitTranslationResponse HabitTranslation { get; set; }
}

public class HabitTranslationResponse
{
    public string Name { get; set; }
    public string Description { get; set; }
    public string HabitItem { get; set; }
    public string LanguageCode { get; set; }
}