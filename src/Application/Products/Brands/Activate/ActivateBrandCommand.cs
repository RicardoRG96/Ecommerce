using Application.Abstractions.Messaging;

namespace Application.Products.Brands.Activate
{
    public sealed record ActivateBrandCommand(long BrandId) : ICommand;
}
