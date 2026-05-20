using System.Text.Json.Serialization;

namespace green_city_sh.Tests.Api.DTO.User;

public class SignUpRequestDto
{
    [JsonPropertyName("name")]
    public string Name { get; set; } = string.Empty;

    [JsonPropertyName("email")]
    public string Email { get; set; } = string.Empty;

    [JsonPropertyName("password")]
    public string Password { get; set; } = string.Empty;

    [JsonPropertyName("isUbs")]
    public bool IsUbs { get; set; } = true;
}
