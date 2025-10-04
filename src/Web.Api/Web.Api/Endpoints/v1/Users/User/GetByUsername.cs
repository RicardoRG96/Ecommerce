
using Application.Abstractions.Messaging;
using Application.Users.Users.GetByUsername;
using SharedKernel;
using Web.Api.Extensions;
using Web.Api.Infrastructure;

namespace Web.Api.Endpoints.v1.Users.User
{
    internal sealed class GetByUsername : IEndpoint
    {
        public void MapEndpoint(IEndpointRouteBuilder app)
        {
            app.MapGet("users/username/{username}", async (
                string username,
                IQueryHandler<GetByUsernameQuery, UserResponse> handler,
                CancellationToken cancellationToken) =>
            {
                GetByUsernameQuery query = new(username);

                Result<UserResponse> result = await handler.Handle(query);

                return result.Match(Results.Ok, CustomResults.Problem);
            })
            .WithTags(Tags.Users);
        }
    }
}
