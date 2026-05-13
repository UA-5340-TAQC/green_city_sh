namespace green_city_sh.Tests.Api.DTO.Events;

public class EventResponse
{
    public int id { get; set; }
    public string title { get; set; }
    public Organizer organizer { get; set; }
    public string creationDate { get; set; }
    public string description { get; set; }
    public List<EventDate> dates { get; set; }
    public List<Tag> tags { get; set; }
    public string titleImage { get; set; }
    public List<string> additionalImages { get; set; }
    public string type { get; set; }
    public bool isRelevant { get; set; }
    public int likes { get; set; }
    public int dislikes { get; set; }
    public int countComments { get; set; }
    public double eventRate { get; set; }
    public double? currentUserGrade { get; set; }
    public bool open { get; set; }
    public bool favorite { get; set; }
    public bool subscribed { get; set; }
}

public class Organizer
{
    public int id { get; set; }
    public string name { get; set; }
    public double? organizerRating { get; set; }
    public string email { get; set; }
}

public class EventDate
{
    public DateTime startDate { get; set; }
    public DateTime finishDate { get; set; }
    public string onlineLink { get; set; }
    public Coordinates coordinates { get; set; }
}

public class Coordinates
{
    public double latitude { get; set; }
    public double longitude { get; set; }
    public string cityEn { get; set; }
    public string cityUk { get; set; }
    public string countryEn { get; set; }
    public string countryUk { get; set; }
    public string formattedAddressEn { get; set; }
    public string formattedAddressUk { get; set; }
}

public class Tag
{
    public int id { get; set; }
    public string nameUk { get; set; }
    public string nameEn { get; set; }
}
