using SharedKernel;

namespace Application.Products.Attributes.Common.Services
{
    internal interface IUniquenessAttributeCodeValidator
    {
        Task<Result> Validate(string code, CancellationToken cancellationToken);
    }
}
