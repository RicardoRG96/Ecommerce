using Application.Abstractions.Messaging;
using Application.Products.Brands.Delete;
using Infrastructure.Access;
using SharedKernel;
using Web.Api.Extensions;
using Web.Api.Infrastructure;

namespace Web.Api.Endpoints.v1.Products.Brand.Delete
{
    internal sealed class Delete : IEndpoint
    {
        public void MapEndpoint(IEndpointRouteBuilder app)
        {
            app.MapDelete("brands/{brandId:long}", async (
                long brandId,
                ICommandHandler<DeleteBrandCommand> handler,
                CancellationToken cancellationToken) =>
            {
                DeleteBrandCommand command = new(brandId);

                Result result = await handler.Handle(command, cancellationToken);

                return result.Match(Results.NoContent, CustomResults.Problem);
            })
            .HasPermission(Permissions.Brands.Delete)
            .WithTags(Tags.Brands);
        }
    }
}
