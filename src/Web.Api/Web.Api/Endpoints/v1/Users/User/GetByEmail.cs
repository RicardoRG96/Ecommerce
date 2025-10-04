
using Application.Abstractions.Messaging;
using Application.Users.Users.GetByEmail;
using SharedKernel;
using Web.Api.Extensions;
using Web.Api.Infrastructure;

namespace Web.Api.Endpoints.v1.Users.User
{
    internal sealed class GetByEmail : IEndpoint
    {
        public void MapEndpoint(IEndpointRouteBuilder app)
        {
            app.MapGet("users/email/{email}", async (
                string email,
                IQueryHandler<GetByEmailQuery, UserResponse> handler,
                CancellationToken cancellationToken) =>
            {
                GetByEmailQuery query = new(email);

                Result<UserResponse> result = await handler.Handle(query);

                return result.Match(Results.Ok, CustomResults.Problem);
            })
            .WithTags(Tags.Users);
        }
    }
}
