using Application.Abstractions.Messaging;
using Application.Users.Users.UpdatePassword;
using Infrastructure.Access;
using SharedKernel;
using Web.Api.Extensions;
using Web.Api.Infrastructure;

namespace Web.Api.Endpoints.v1.Users.User.UpdatePassword
{
    internal sealed class UpdatePassword : IEndpoint
    {
        public void MapEndpoint(IEndpointRouteBuilder app)
        {
            app.MapPut("users/me/password/{userId:long}", async (
                long userId,
                UpdatePasswordRequest request,
                ICommandHandler<UpdatePasswordCommand> handler,
                CancellationToken cancellationToken) =>
            {
                UpdatePasswordCommand command = new(userId, request.CurrentPassword, request.NewPassword);

                Result result = await handler.Handle(command, cancellationToken);

                return result.Match(Results.NoContent, CustomResults.Problem);
            })
            .HasPermission(Permissions.Users.UpdatePassword)
            .WithTags(Tags.Users);
        }
    }
}
