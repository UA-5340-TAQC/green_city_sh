using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace green_city_sh.Tests.Api.DTO.User;

public class CoordinatesDto
{
    [JsonPropertyName("latitude")]
    public double Latitude { get; set; }

    [JsonPropertyName("longitude")]
    public double Longitude { get; set; }
}

public class EmailPreferenceDto
{
    [JsonPropertyName("emailPreference")]
    public string EmailPreference { get; set; } = string.Empty;

    [JsonPropertyName("periodicity")]
    public string Periodicity { get; set; } = string.Empty;
}

public class ProfileUpdateRequestDto
{
    [JsonPropertyName("name")]
    public string Name { get; set; } = string.Empty;

    [JsonPropertyName("socialNetworks")]
    public List<string> SocialNetworks { get; set; } = new();

    [JsonPropertyName("showLocation")]
    public string ShowLocation { get; set; } = "PUBLIC";

    [JsonPropertyName("showEcoPlace")]
    public string ShowEcoPlace { get; set; } = "PUBLIC";

    [JsonPropertyName("showToDoList")]
    public string ShowToDoList { get; set; } = "PUBLIC";

    [JsonPropertyName("coordinates")]
    public CoordinatesDto Coordinates { get; set; } = new();

    [JsonPropertyName("emailPreferences")]
    public List<EmailPreferenceDto> EmailPreferences { get; set; } = new();
}
