using Application.Abstractions.Messaging;

namespace Application.Products.Brands.Delete
{
    public sealed record DeleteBrandCommand(long BrandId) : ICommand;
}
