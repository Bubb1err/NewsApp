namespace NewsApp.Shared.Models.Dto.User;

public class RegisterDto
{
    public string name { get; set; }
    public string email{ get; set; }
    public string password{ get; set; }
    public string confirmPassword { get; set; }
}