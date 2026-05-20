using System.Text.Json.Serialization;

namespace green_city_sh.Tests.Api.DTO.User;

public class UserProfileExternalDto
{
    [JsonPropertyName("userId")]
    public long UserId { get; set; }

    [JsonPropertyName("email")]
    public string Email { get; set; } = string.Empty;

    [JsonPropertyName("profilePicturePath")]
    public string? ProfilePicturePath { get; set; }

    [JsonPropertyName("userCredo")]
    public string? UserCredo { get; set; }

    [JsonPropertyName("userRating")]
    public double? UserRating { get; set; }

    [JsonPropertyName("userLocationDto")]
    public UserLocationDto? UserLocationDto { get; set; }
}
