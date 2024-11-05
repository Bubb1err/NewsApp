using System.ComponentModel.DataAnnotations;
using MediatR;
using NewsApp.Shared.Models.Base;
using NewsApp.Shared.Models.Dto.User;

namespace NewsApp.API.Application.Authentication.Register
{
    public sealed class RegisterCommand(string name, string email, string password, string confirmPassword)
        : IRequest<DataApiResponseDto<UserDto>>
    {
        public string Name { get; } = name;

        [EmailAddress]
        public string Email { get; } = email;

        [DataType(DataType.Password)]
        public string Password { get; } = password;

        [DataType(DataType.Password)]
        [Compare(nameof(Password))]
        public string ConfirmPassword { get; } = confirmPassword;
    }
}
