using Application.Abstractions.Messaging;
using Application.Products.AttributeValues.Common.Mappers;
using Application.Products.AttributeValues.GetValuesByAttribute;
using Infrastructure.Access;
using SharedKernel;
using Web.Api.Extensions;
using Web.Api.Infrastructure;

namespace Web.Api.Endpoints.v1.Products.AttributeValue.Get
{
    internal sealed class GetByAttributeId : IEndpoint
    {
        public void MapEndpoint(IEndpointRouteBuilder app)
        {
            app.MapGet("attribute-values/attribute/{attributeId:long}/values", async (
                long attributeId,
                IQueryHandler<GetValuesByAttributeQuery, List<AttributeValueResponse>> handler,
                CancellationToken cancellationToken) =>
            {
                GetValuesByAttributeQuery query = new(attributeId);

                Result<List<AttributeValueResponse>> result = await handler.Handle(query, cancellationToken);

                return result.Match(Results.Ok, CustomResults.Problem);
            })
            .HasPermission(Permissions.AttributeValues.Read)
            .WithTags(Tags.AttributeValues);
        }
    }
}
