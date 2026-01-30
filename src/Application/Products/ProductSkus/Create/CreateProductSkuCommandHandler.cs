using Application.Abstractions.Data.Repositories.Products;
using Application.Abstractions.Data.UnitOfWork;
using Application.Abstractions.Messaging;
using Application.Products.ProductSkus.Common.Services;
using Domain.Entities.Products;
using SharedKernel;

namespace Application.Products.ProductSkus.Create
{
    internal sealed class CreateProductSkuCommandHandler : ICommandHandler<CreateProductSkuCommand, long>
    {
        private readonly IProductSkuRepository _productSkuRepository;
        private readonly IProductSkuValidator _validator;
        private readonly ISkuGenerator _skuGenerator;
        private readonly SkuGenerationContextBuilder _skuGenerationContextBuilder;
        private readonly IUnitOfWork _unitOfWork;

        public CreateProductSkuCommandHandler(
            IProductSkuRepository productSkuRepository, 
            IProductSkuValidator productSkuValidator, 
            ISkuGenerator skuGenerator,
            SkuGenerationContextBuilder skuGenerationContextBuilder, 
            IUnitOfWork unitOfWork)
        {
            _productSkuRepository = productSkuRepository;
            _validator = productSkuValidator;
            _skuGenerator = skuGenerator;
            _skuGenerationContextBuilder = skuGenerationContextBuilder;
            _unitOfWork = unitOfWork;
        }

        public async Task<Result<long>> Handle(CreateProductSkuCommand command, CancellationToken cancellationToken)
        {
            Result productPublishedValidation = await _validator.ValidateProductIsPublishedAsync(
                command.ProductId, 
                cancellationToken);

            if (productPublishedValidation.IsFailure)
            {
                return Result.Failure<long>(productPublishedValidation.Error);
            }

            Result barCodeUniquenessValidation = await _validator.ValidateBarCodeIsUnique(
                barCode: command.BarCode, 
                cancellationToken: cancellationToken);

            if (barCodeUniquenessValidation.IsFailure)
            {
                return Result.Failure<long>(barCodeUniquenessValidation.Error);
            }

            SkuGenerationContext context = await _skuGenerationContextBuilder.BuildForProductAsync(
                command.ProductId, 
                cancellationToken);

            string generatedSku = _skuGenerator.Generate(context);

            Result skuUniquenessValidation = await _validator.ValidateSkuCodeIsUnique(
                skuCode: generatedSku, 
                cancellationToken: cancellationToken);

            if (skuUniquenessValidation.IsFailure)
            {
                return Result.Failure<long>(skuUniquenessValidation.Error);
            }

            ProductSku productSku = ProductSku.Create(
                command.ProductId,
                generatedSku,
                command.BarCode,
                command.Price,
                command.Cost,
                command.Weight,
                command.Length,
                command.Width,
                command.Height,
                command.IsActive,
                command.DisplayOrder);

            await _productSkuRepository.AddAsync(productSku, cancellationToken);

            await _unitOfWork.SaveChangesAsync(cancellationToken);

            return Result.Success(productSku.Id);
        }
    }
}
