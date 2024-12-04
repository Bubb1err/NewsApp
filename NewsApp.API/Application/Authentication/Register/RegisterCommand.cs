using System.ComponentModel.DataAnnotations;
using MediatR;
using NewsApp.Shared.Models.Base;
using NewsApp.Shared.Models.Dto.User;

namespace NewsApp.API.Application.Authentication.Register
{
    public class RegisterCommand : IRequest<DataApiResponseDto<UserDto>>
    {
        [Required]
        public string Name { get; set; }

        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [Required]
        [DataType(DataType.Password)]
        [Compare(nameof(Password))]
        public string ConfirmPassword { get; set; }

        // Default constructor for deserialization
        public RegisterCommand() { }

        // Optional: Use the constructor for convenience
        public RegisterCommand(string name, string email, string password, string confirmPassword)
        {
            Name = name;
            Email = email;
            Password = password;
            ConfirmPassword = confirmPassword;
        }
    }

}
