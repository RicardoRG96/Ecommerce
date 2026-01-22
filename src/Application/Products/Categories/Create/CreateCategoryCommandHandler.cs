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

            Category category = new()
            {
                ParentId = command.ParentId,
                Name = command.Name,
                Slug = SlugGenerator.GenerateSlug(command.Name),
                Description = command.Description,
                ImageUrl = command.ImageUrl,
                Icon = command.Icon,
                DisplayOrder = command.DisplayOrder,
                IsActive = command.IsActive,
                IsVisibleInMenu = command.IsVisibleInMenu,
                MetaTitle = command.MetaTitle,
                MetaDescription = command.MetaDescription,
                MetaKeywords = command.MetaKeywords
            };

            await _categoryRepository.AddAsync(category, cancellationToken);
            await _unitOfWork.SaveChangesAsync(cancellationToken);

            return Result.Success(category.Id);
        }
    }
}
