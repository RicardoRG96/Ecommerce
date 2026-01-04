
using Application.Abstractions.Messaging;
using Application.Users.Users.Login;
using SharedKernel;
using Web.Api.Extensions;
using Web.Api.Infrastructure;

namespace Web.Api.Endpoints.v1.Users.User.Login
{
    internal sealed class Login : IEndpoint
    {
        public void MapEndpoint(IEndpointRouteBuilder app)
        {
            app.MapPost("users/login", async (
                LoginUserRequest request,
                ICommandHandler<LoginUserCommand, Dictionary<string, string>> handler,
                CancellationToken cancellationToken) =>
            {
                LoginUserCommand command = new(request.Email, request.Password);

                Result<Dictionary<string, string>> result = await handler.Handle(command, cancellationToken);

                return result.Match(Results.Ok, CustomResults.Problem);
            })
            .WithTags(Tags.Users);
        }
    }
}
