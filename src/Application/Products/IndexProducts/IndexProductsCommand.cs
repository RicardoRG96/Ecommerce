using Application.Abstractions.Messaging;

namespace Application.Products.IndexProducts
{
    public sealed record IndexProductsCommand : ICommand<bool>;
}
