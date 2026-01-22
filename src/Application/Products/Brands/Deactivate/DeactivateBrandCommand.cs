using Application.Abstractions.Messaging;

namespace Application.Products.Brands.Deactivate
{
    public sealed record DeactivateBrandCommand(long BrandId) : ICommand;
}
