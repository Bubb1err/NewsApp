using Analytics.Web.Exceptions;
using MediatR;
using Microsoft.AspNetCore.Identity;
using NewsApp.API.Data.Entities;

namespace NewsApp.API.Application.Authentication.Login
{
    internal sealed class LoginCommandHandler : IRequestHandler<LoginCommand>
    {
        private readonly SignInManager<User> _signInManager;
        private readonly UserManager<User> _userManager;

        public LoginCommandHandler(SignInManager<User> signInManager, UserManager<User> userManager)
        {
            _signInManager = signInManager;
            _userManager = userManager;
        }

        public async Task Handle(LoginCommand request, CancellationToken cancellationToken)
        {
            var user = await _userManager.FindByEmailAsync(request.Email);
            if (user == null)
            {
                throw new KeyNotFoundException("User not found.");
            }

            var signInResult = await _signInManager.PasswordSignInAsync(request.Email, request.Password, isPersistent: false, lockoutOnFailure: false);

            if (!signInResult.Succeeded)
            {
                throw new ExecutingException("User sign in failed.", System.Net.HttpStatusCode.BadRequest);
            }
        }
    }
}
