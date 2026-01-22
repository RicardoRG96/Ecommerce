using Application.Abstractions.Data.Repositories.Products;
using Application.Abstractions.Data.UnitOfWork;
using Application.Abstractions.Messaging;
using Domain.Entities.Products;
using Domain.Errors.Products;
using SharedKernel;

namespace Application.Products.Categories.Update
{
    internal sealed class UpdateCategoryCommandHandler : ICommandHandler<UpdateCategoryCommand>
    {
        private readonly ICategoryRepository _categoryRepository;
        private readonly IUnitOfWork _unitOfWork;

        public UpdateCategoryCommandHandler(ICategoryRepository categoryRepository, IUnitOfWork unitOfWork)
        {
            _categoryRepository = categoryRepository;
            _unitOfWork = unitOfWork;
        }

        public async Task<Result> Handle(UpdateCategoryCommand command, CancellationToken cancellationToken)
        {
            Category? category = await _categoryRepository.GetByIdAsync(command.CategoryId, cancellationToken);

            if (category is null)
            {
                return Result.Failure(CategoryErrors.NotFound(command.CategoryId));
            }

            Category? existingCategory = await _categoryRepository.GetByNameAsync(command.Name, cancellationToken);

            if (existingCategory is not null && 
                existingCategory.Id != category.Id)
            {
                return Result.Failure(CategoryErrors.CategoryAlreadyExists);
            }

            category.Name = command.Name;
            category.Slug = SlugGenerator.GenerateSlug(command.Name);
            category.Description = command.Description;
            category.ImageUrl = command.ImageUrl;
            category.Icon = command.Icon;
            category.DisplayOrder = command.DisplayOrder;
            category.IsVisibleInMenu = command.IsVisibleInMenu;
            category.MetaTitle = command.MetaTitle;
            category.MetaDescription = command.MetaDescription;
            category.MetaKeywords = command.MetaKeywords;

            _categoryRepository.Update(category);

            await _unitOfWork.SaveChangesAsync(cancellationToken);

            return Result.Success();
        }
    }
}
