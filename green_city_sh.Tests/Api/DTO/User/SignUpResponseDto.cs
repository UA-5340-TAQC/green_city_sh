using System.Text.Json.Serialization;

namespace green_city_sh.Tests.Api.DTO.User;

public class SignUpResponseDto
{
    [JsonPropertyName("userId")]
    public long UserId { get; set; }

    [JsonPropertyName("username")]
    public string Username { get; set; } = string.Empty;

    [JsonPropertyName("email")]
    public string Email { get; set; } = string.Empty;

    [JsonPropertyName("ownRegistrations")]
    public bool OwnRegistrations { get; set; }
}
