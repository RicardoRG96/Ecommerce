using Application.Abstractions.Messaging;

namespace Application.Products.Brands.Delete
{
    public sealed class DeleteBrandCommand(long BrandId) : ICommand;
}
