using Application.Abstractions.Messaging;
using Application.Products.AttributeValues.Common.Mappers;
using Application.Products.AttributeValues.GetById;
using Infrastructure.Access;
using SharedKernel;
using Web.Api.Extensions;
using Web.Api.Infrastructure;

namespace Web.Api.Endpoints.v1.Products.AttributeValue.Get
{
    internal sealed class GetById : IEndpoint
    {
        public void MapEndpoint(IEndpointRouteBuilder app)
        {
            app.MapGet("attribute-values/{id:long}", async (
                long id,
                IQueryHandler<GetAttributeValueByIdQuery, AttributeValueResponse> handler,
                CancellationToken cancellationToken) =>
            {
                GetAttributeValueByIdQuery query = new(id);

                Result<AttributeValueResponse> result = await handler.Handle(query, cancellationToken);

                return result.Match(Results.Ok, CustomResults.Problem);
            })
            .HasPermission(Permissions.AttributeValues.Read)
            .WithTags(Tags.AttributeValues);
        }
    }
}
