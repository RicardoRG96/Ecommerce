namespace Application.Products.ProductSkus.Common.Services
{
    public interface ISkuGenerator
    {
        string Generate(SkuGenerationContext context);
    }
}
