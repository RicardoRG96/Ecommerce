
using Application.Abstractions.Messaging;
using Application.Users.Users.GetById;
using SharedKernel;
using Web.Api.Extensions;

namespace Web.Api.Endpoints.v1.Users.User
{
    public class GetById : IEndpoint
    {
        public void MapEndpoint(IEndpointRouteBuilder app)
        {
            app.MapGet("users/{userId}", async (
                long userId,
                IQueryHandler<GetUserByIdQuery, UserResponse> handler,
                CancellationToken cancellationToken) =>
            {
                GetUserByIdQuery query = new(userId);
                Result<UserResponse> result = await handler.Handle(query);

                return result.Match(Results.Ok, CustomResults.Problem);
            })
            .WithTags(Tags.Users);
        }
    }
}
