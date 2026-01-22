using Application.Abstractions.Messaging;
using Application.Products.Brands.Activate;
using Infrastructure.Access;
using SharedKernel;
using Web.Api.Extensions;
using Web.Api.Infrastructure;

namespace Web.Api.Endpoints.v1.Products.Brand.Activate
{
    internal sealed class ActivateBrand : IEndpoint
    {
        public void MapEndpoint(IEndpointRouteBuilder app)
        {
            app.MapPatch("brands/{brandId:long}/activate", async (
                long brandId,
                ICommandHandler<ActivateBrandCommand> handler,
                CancellationToken cancellationToken) =>
            {
                ActivateBrandCommand command = new(brandId);

                Result result = await handler.Handle(command, cancellationToken);

                return result.Match(Results.NoContent, CustomResults.Problem);
            })
            .HasPermission(Permissions.Brands.Update)
            .WithTags(Tags.Brands);
        }
    }
}
