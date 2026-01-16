using FluentValidation;

namespace Application.Users.RefreshTokens.Revoke
{
    public sealed class RevokeRefreshTokenCommandValidator : AbstractValidator<RevokeRefreshTokenCommand>
    {
        public RevokeRefreshTokenCommandValidator()
        {
            RuleFor(c => c.UserId).NotEmpty();
        }
    }
}
