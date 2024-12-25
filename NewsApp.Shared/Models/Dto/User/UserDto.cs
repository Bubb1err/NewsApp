using NewsApp.Shared.Constants;

namespace NewsApp.Shared.Models.Dto.User;

public class UserDto
{
    public string Id { get; set; }

    public string Email { get; set; } = string.Empty;

    public UserState State { get; set; } = UserState.Unknown;

    public string Role { get; set; } = string.Empty;

    public string Name { get; set; } = string.Empty;
}