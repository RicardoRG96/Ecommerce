using Application.Abstractions.Messaging;
using Application.Products.Brands.Deactivate;
using Infrastructure.Access;
using SharedKernel;
using Web.Api.Extensions;
using Web.Api.Infrastructure;

namespace Web.Api.Endpoints.v1.Products.Brand.Deactivate
{
    internal sealed class DeactivateBrand : IEndpoint
    {
        public void MapEndpoint(IEndpointRouteBuilder app)
        {
            app.MapPatch("brands/{brandId:long}/deactivate", async (
                long brandId,
                ICommandHandler<DeactivateBrandCommand> handler,
                CancellationToken cancellationToken) =>
            {
                DeactivateBrandCommand command = new(brandId);

                Result result = await handler.Handle(command, cancellationToken);

                return result.Match(Results.NoContent, CustomResults.Problem);
            })
            .HasPermission(Permissions.Brands.Update)
            .WithTags(Tags.Brands);
        }
    }
}
