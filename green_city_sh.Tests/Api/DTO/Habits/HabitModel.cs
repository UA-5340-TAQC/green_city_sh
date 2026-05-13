
namespace green_city_sh.Tests.Api.DTO.Habits;

public class HabitModel
{
    public long Id { get; set; }
    public int Complexity { get; set; }
    public int DefaultDuration { get; set; }
    public List<HabitTranslation> HabitTranslations { get; set; } = new();
    public List<CustomToDoListItem> CustomToDoListItemDto { get; set; } = new();
    public List<int> TagIds { get; set; } = new();
    public List<object> FriendsToInvite { get; set; } = new();
    public string? Image { get; set; } = "";
}

public class CustomToDoListItem
{
    public long? Id { get; set; }
    public string Text { get; set; } = string.Empty;
    public string? Status { get; set; }
}

public class HabitTranslation
{
    public string Name { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public string HabitItem { get; set; } = string.Empty;
    public string LanguageCode { get; set; } = string.Empty;
}
