using Application.Abstractions.Messaging;
using Application.Products.Attributes.GetAll;
using Infrastructure.Access;
using SharedKernel;
using Web.Api.Extensions;
using Web.Api.Infrastructure;

namespace Web.Api.Endpoints.v1.Products.Attribute.Get
{
    internal sealed class GetAll : IEndpoint
    {
        public void MapEndpoint(IEndpointRouteBuilder app)
        {
            app.MapGet("attributes", async (
                IQueryHandler<GetAllAttributesQuery, List<AttributeResponse>> handler,
                CancellationToken cancellationToken) =>
            {
                GetAllAttributesQuery query = new();

                Result<List<AttributeResponse>> result = await handler.Handle(query, cancellationToken);

                return result.Match(Results.Ok, CustomResults.Problem);
            })
            .HasPermission(Permissions.Attributes.Read)
            .WithTags(Tags.Attributes);
        }
    }
}
