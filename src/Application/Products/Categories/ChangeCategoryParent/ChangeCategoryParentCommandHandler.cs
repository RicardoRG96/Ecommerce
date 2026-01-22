using Application.Abstractions.Data.Repositories.Products;
using Application.Abstractions.Data.UnitOfWork;
using Application.Abstractions.Messaging;
using Domain.Entities.Products;
using Domain.Errors.Products;
using SharedKernel;

namespace Application.Products.Categories.ChangeCategoryParent
{
    internal sealed class ChangeCategoryParentCommandHandler : ICommandHandler<ChangeCategoryParentCommand>
    {
        private readonly ICategoryRepository _categoryRepository;
        private readonly IUnitOfWork _unitOfWork;

        public ChangeCategoryParentCommandHandler(ICategoryRepository categoryRepository, IUnitOfWork unitOfWork)
        {
            _categoryRepository = categoryRepository;
            _unitOfWork = unitOfWork;
        }

        public async Task<Result> Handle(ChangeCategoryParentCommand command, CancellationToken cancellationToken)
        {
            Category? category = await _categoryRepository.GetByIdAsync(command.CategoryId, cancellationToken);

            if (category is null)
            {
                return Result.Failure(CategoryErrors.NotFound(command.CategoryId));
            }

            category.ParentId = command.NewParentId;

            _categoryRepository.Update(category);

            await _unitOfWork.SaveChangesAsync(cancellationToken);

            return Result.Success();
        }
    }
}
