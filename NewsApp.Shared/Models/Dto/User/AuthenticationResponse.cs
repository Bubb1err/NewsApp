namespace NewsApp.Shared.Models.Dto.User;

public class AuthenticationResponseDto
{
    public string JwtToken { get; set; }
    public Guid UserId { get; set; }
}
