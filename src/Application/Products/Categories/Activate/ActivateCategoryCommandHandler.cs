using Application.Abstractions.Data.Repositories.Products;
using Application.Abstractions.Data.UnitOfWork;
using Application.Abstractions.Messaging;
using Domain.Entities.Products;
using Domain.Errors.Products;
using SharedKernel;

namespace Application.Products.Categories.Activate
{
    internal sealed class ActivateCategoryCommandHandler : ICommandHandler<ActivateCategoryCommand>
    {
        private readonly ICategoryRepository _categoryRepository;
        private readonly IUnitOfWork _unitOfWork;

        public ActivateCategoryCommandHandler(ICategoryRepository categoryRepository, IUnitOfWork unitOfWork)
        {
            _categoryRepository = categoryRepository;
            _unitOfWork = unitOfWork;
        }

        public async Task<Result> Handle(ActivateCategoryCommand command, CancellationToken cancellationToken)
        {
            Category? category = await _categoryRepository.GetByIdAsync(command.CategoryId, cancellationToken);

            if (category is null)
            {
                return Result.Failure(CategoryErrors.NotFound(command.CategoryId));
            }

            category.IsActive = true;

            _categoryRepository.Update(category);

            await _unitOfWork.SaveChangesAsync(cancellationToken);

            return Result.Success();
        }
    }
}
