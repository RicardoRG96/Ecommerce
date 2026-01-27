using SharedKernel;

namespace Application.Products.Attributes.Common.Services
{
    internal interface IUniquenessAttributeCodeValidator
    {
        Task<Result> ValidateAsync(
            string code, 
            long? excludeAttributeId = null, 
            CancellationToken cancellationToken = default);
    }
}
