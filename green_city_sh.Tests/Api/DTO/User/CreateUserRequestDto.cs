using System.Text.Json.Serialization;

namespace green_city_sh.Tests.Api.DTO.User;

public class CreateUserRequestDto
{
    [JsonPropertyName("id")]
    public long Id { get; set; }

    [JsonPropertyName("email")]
    public string Email { get; set; } = string.Empty;

    [JsonPropertyName("name")]
    public string Name { get; set; } = string.Empty;

    [JsonPropertyName("profilePicturePath")]
    public string? ProfilePicturePath { get; set; }
}
