
using Application.Abstractions.Messaging;
using Application.Users.Users.Update;
using SharedKernel;
using Web.Api.Extensions;
using Web.Api.Infrastructure;

namespace Web.Api.Endpoints.v1.Users.User
{
    internal sealed class Update : IEndpoint
    {
        public sealed class Request
        {
            public string? Avatar { get; set; }
            public string FristName { get; set; }
            public string LastName { get; set; }
            public string PhoneNumber { get; set; }
        }

        public void MapEndpoint(IEndpointRouteBuilder app)
        {
            app.MapPut("users/{userId}", async (
                long userId,
                Request request,
                ICommandHandler<UpdateUserCommand> handler,
                CancellationToken cancellationToken) =>
            {
                UpdateUserCommand command = new()
                {
                    UserId = userId,
                    Avatar = request.Avatar,
                    FirstName = request.FristName,
                    LastName = request.LastName,
                    PhoneNumber = request.PhoneNumber
                };

                Result result = await handler.Handle(command);

                return result.Match(Results.NoContent, CustomResults.Problem);
            })
            .WithTags(Tags.Users);
        }
    }
}
