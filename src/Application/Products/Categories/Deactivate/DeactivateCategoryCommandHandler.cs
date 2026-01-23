using Application.Abstractions.Data.Repositories.Products;
using Application.Abstractions.Data.UnitOfWork;
using Application.Abstractions.Messaging;
using Domain.Entities.Products;
using Domain.Errors.Products;
using SharedKernel;

namespace Application.Products.Categories.Deactivate
{
    internal sealed class DeactivateCategoryCommandHandler : ICommandHandler<DeactivateCategoryCommand>
    {
        private readonly ICategoryRepository _categoryRepository;
        private readonly IUnitOfWork _unitOfWork;

        public DeactivateCategoryCommandHandler(ICategoryRepository categoryRepository, IUnitOfWork unitOfWork)
        {
            _categoryRepository = categoryRepository;
            _unitOfWork = unitOfWork;
        }

        public async Task<Result> Handle(DeactivateCategoryCommand command, CancellationToken cancellationToken)
        {
            Category? category = await _categoryRepository.GetByIdAsync(command.CategoryId, cancellationToken);

            if (category is null)
            {
                return Result.Failure(CategoryErrors.NotFound(command.CategoryId));
            }

            if (!category.IsActive)
            {
                return Result.Success();
            }

            category.IsActive = false;

            _categoryRepository.Update(category);

            await _unitOfWork.SaveChangesAsync(cancellationToken);

            return Result.Success();
        }
    }
}
