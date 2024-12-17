using System.ComponentModel.DataAnnotations;

namespace NewsApp.Shared.Models.Dto.User;

public class RegisterDto
{
    public string Name { get; set; }
    public string Email{ get; set; }
    
    
    [Required(ErrorMessage = "Password is required.")]
    [StringLength(100, ErrorMessage = "The password must be at least 8 characters long.", MinimumLength = 8)]
    [RegularExpression("(?=.*[a-z])(?=.*[A-Z])(?=.*\\d)(?=.*[@$!%*?&])[A-Za-z\\d@$!%*?&]{8,}",
        ErrorMessage = "Password must have at least one uppercase letter, one lowercase letter, one digit, and one special character.")]
    public string Password{ get; set; }
    
    [Required(ErrorMessage = "Please confirm your password.")]
    [Compare("password", ErrorMessage = "Passwords do not match.")]
    public string ConfirmPassword { get; set; }
}