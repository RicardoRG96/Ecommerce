using FluentValidation;

namespace Application.Users.RefreshTokens.Login
{
    internal sealed class LoginWithRefreshTokenCommandValidator : AbstractValidator<LoginWithRefreshTokenCommand>
    {
        public LoginWithRefreshTokenCommandValidator()
        {
            RuleFor(c => c.RefreshToken).NotEmpty();
        }
    }
}
