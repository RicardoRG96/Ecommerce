using Application.Abstractions.Data.Repositories.Products;
using Application.Abstractions.Messaging;
using Domain.Entities.Products;
using Domain.Errors.Products;
using SharedKernel;

namespace Application.Products.Categories.GetCategoryTree
{
    internal sealed class GetCategoryTreeQueryHandler : IQueryHandler<GetCategoryTreeQuery, CategoryTreeResponse>
    {
        private readonly ICategoryRepository _categoryRepository;

        public GetCategoryTreeQueryHandler(ICategoryRepository categoryRepository)
        {
            _categoryRepository = categoryRepository;
        }

        public async Task<Result<CategoryTreeResponse>> Handle(GetCategoryTreeQuery query, CancellationToken cancellationToken)
        {
            Category? category = await _categoryRepository.GetByIdAsync(query.CategoryId, cancellationToken);

            if (category is null)
            {
                return Result.Failure<CategoryTreeResponse>(CategoryErrors.NotFound(query.CategoryId));
            }

            static CategoryTreeResponse BuildCategoryTree(Category cat)
            {
                return new CategoryTreeResponse
                {
                    Id = cat.Id,
                    Name = cat.Name,
                    Slug = cat.Slug,
                    ImageUrl = cat.ImageUrl,
                    Icon = cat.Icon,
                    DisplayOrder = cat.DisplayOrder,
                    Children = cat.Children?
                        .OrderBy(c => c.DisplayOrder)
                        .Where(c => c.IsActive)
                        .Where(c => c.IsVisibleInMenu)
                        .Select(BuildCategoryTree)
                        .ToList()
                };
            }

            return Result.Success(BuildCategoryTree(category));
        }
    }
}
