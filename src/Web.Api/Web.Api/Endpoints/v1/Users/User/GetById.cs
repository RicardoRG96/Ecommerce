
using Application.Abstractions.Messaging;
using Application.Users.Users.GetById;
using Asp.Versioning;
using Asp.Versioning.Builder;
using SharedKernel;
using Web.Api.Extensions;
using Web.Api.Infrastructure;

namespace Web.Api.Endpoints.v1.Users.User
{
    public class GetById : IEndpoint
    {
        public void MapEndpoint(IEndpointRouteBuilder app)
        {
            ApiVersionSet apiVersionSet = app.NewApiVersionSet()
                .HasApiVersion(new ApiVersion(1))
                .ReportApiVersions()
                .Build();

            app.MapGet("api/v{apiVersion:apiVersion}/users/{userId}", async (
                long userId,
                IQueryHandler<GetUserByIdQuery, UserResponse> handler,
                CancellationToken cancellationToken) =>
            {
                GetUserByIdQuery query = new(userId);

                Result<UserResponse> result = await handler.Handle(query);

                return result.Match(Results.Ok, CustomResults.Problem);
            })
            .WithApiVersionSet(apiVersionSet)
            .WithTags(Tags.Users);
        }
    }
}
