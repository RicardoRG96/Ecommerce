using Application.Abstractions.Data.Repositories.Products;
using Application.Abstractions.Data.UnitOfWork;
using Application.Abstractions.Messaging;
using Domain.Entities.Products;
using Domain.Errors.Products;
using SharedKernel;

namespace Application.Products.Brands.Delete
{
    internal sealed class DeleteBrandCommandHandler : ICommandHandler<DeleteBrandCommand>
    {
        private readonly IBrandRepository _brandRepository;
        private readonly IUnitOfWork _unitOfWork;

        public DeleteBrandCommandHandler(IBrandRepository brandRepository, IUnitOfWork unitOfWork)
        {
            _brandRepository = brandRepository;
            _unitOfWork = unitOfWork;
        }

        public async Task<Result> Handle(DeleteBrandCommand command, CancellationToken cancellationToken)
        {
            Brand? brand = await _brandRepository.GetByIdAsync(command.BrandId, cancellationToken);

            if (brand is null)
            {
                return Result.Failure(BrandErrors.NotFound(command.BrandId));
            }

            _brandRepository.Delete(brand);

            await _unitOfWork.SaveChangesAsync(cancellationToken);

            return Result.Success();
        }
    }
}
