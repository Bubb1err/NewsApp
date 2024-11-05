using System.ComponentModel.DataAnnotations;
using MediatR;

namespace NewsApp.API.Application.Authentication.Login
{
    public sealed class LoginCommand(string email, string password) : IRequest
    {
        [EmailAddress]
        public string Email { get; } = email;

        [DataType(DataType.Password)]
        public string Password { get; } = password;
    }
}
