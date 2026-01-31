using Application.Abstractions.Data.Repositories.Products;
using Application.Abstractions.Data.UnitOfWork;
using Application.Abstractions.Messaging;
using Domain.Entities.Products;
using Domain.Errors.Products;
using SharedKernel;

namespace Application.Products.Categories.Create
{
    internal sealed class CreateCategoryCommandHandler : ICommandHandler<CreateCategoryCommand, long>
    {
        private readonly ICategoryRepository _categoryRepository;
        private readonly IUnitOfWork _unitOfWork;

        public CreateCategoryCommandHandler(
            ICategoryRepository categoryRepository, 
            IUnitOfWork unitOfWork)
        {
            _categoryRepository = categoryRepository;
            _unitOfWork = unitOfWork;
        }

        public async Task<Result<long>> Handle(CreateCategoryCommand command, CancellationToken cancellationToken)
        {
            Category? existingCategory = await _categoryRepository.GetByNameAsync(command.Name, cancellationToken);

            if (existingCategory is not null)
            {
                return Result.Failure<long>(CategoryErrors.CategoryAlreadyExists);
            }

            Category category = Category.Create(
                parentId: command.ParentId,
                name: command.Name,
                description: command.Description,
                imageUrl: command.ImageUrl,
                icon: command.Icon,
                displayOrder: command.DisplayOrder,
                isActive: command.IsActive,
                isVisibleInMenu: command.IsVisibleInMenu,
                metaTitle: command.MetaTitle,
                metaDescription: command.MetaDescription,
                metaKeywords: command.MetaKeywords
            );

            await _categoryRepository.AddAsync(category, cancellationToken);
            await _unitOfWork.SaveChangesAsync(cancellationToken);

            return Result.Success(category.Id);
        }
    }
}
