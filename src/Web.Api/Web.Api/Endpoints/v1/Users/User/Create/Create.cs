using Application.Abstractions.Messaging;
using Application.Users.Users.Create;
using SharedKernel;
using Web.Api.Extensions;
using Web.Api.Infrastructure;

namespace Web.Api.Endpoints.v1.Users.User.Create
{
    internal sealed class Create : IEndpoint
    {
        public void MapEndpoint(IEndpointRouteBuilder app)
        {
            app.MapPost("users", async (
                CreateUserRequest request,
                ICommandHandler<CreateUserCommand, long> handler,
                CancellationToken cancellationToken) =>
            {
                CreateUserCommand command = new(
                    request.Avatar,
                    request.FirstName,
                    request.LastName,
                    request.Username,
                    request.Email,
                    request.Password,
                    request.DateOfBirth,
                    request.PhoneNumber);

                Result<long> result = await handler.Handle(command, cancellationToken);

                return result.Match(Results.Ok, CustomResults.Problem);
            })
            .WithTags(Tags.Users);
        }
    }
}
