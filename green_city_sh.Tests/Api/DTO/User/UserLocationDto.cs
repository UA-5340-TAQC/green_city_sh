using System.Text.Json.Serialization;

namespace green_city_sh.Tests.Api.DTO.User;

public class UserLocationDto
{
    [JsonPropertyName("id")]
    public long Id { get; set; }

    [JsonPropertyName("cityEn")]
    public string CityEn { get; set; } = string.Empty;

    [JsonPropertyName("cityUk")]
    public string CityUk { get; set; } = string.Empty;

    [JsonPropertyName("countryEn")]
    public string CountryEn { get; set; } = string.Empty;

    [JsonPropertyName("countryUk")]
    public string CountryUk { get; set; } = string.Empty;

    [JsonPropertyName("regionEn")]
    public string RegionEn { get; set; } = string.Empty;

    [JsonPropertyName("regionUk")]
    public string RegionUk { get; set; } = string.Empty;

    [JsonPropertyName("latitude")]
    public double? Latitude { get; set; }

    [JsonPropertyName("longitude")]
    public double? Longitude { get; set; }
}
