using SharedKernel;

namespace Application.Products.Attributes.Common.Services
{
    public interface IUniquenessAttributeCodeValidator
    {
        Task<Result> ValidateAsync(
            string code, 
            long? excludeAttributeId = null, 
            CancellationToken cancellationToken = default);
    }
}
