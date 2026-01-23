using Application.Abstractions.Data.Repositories.Products;
using Application.Abstractions.Messaging;
using Domain.Entities.Products;
using SharedKernel;

namespace Application.Products.Categories.GetCategoryTree
{
    internal sealed class GetCategoryTreeQueryHandler 
        : IQueryHandler<GetCategoryTreeQuery, List<CategoryTreeResponse>>
    {
        private readonly ICategoryRepository _categoryRepository;

        public GetCategoryTreeQueryHandler(ICategoryRepository categoryRepository)
        {
            _categoryRepository = categoryRepository;
        }

        public async Task<Result<List<CategoryTreeResponse>>> Handle(GetCategoryTreeQuery query, CancellationToken cancellationToken)
        {
            List<Category> categories = await _categoryRepository.GetAllAsync(cancellationToken);

            return Result.Success(BuildCategoryTree(categories, parentId: null));
        }

        private static List<CategoryTreeResponse> BuildCategoryTree(
            List<Category> categories,
            long? parentId)
        {
            return categories
                .Where(c => c.ParentId == parentId)
                .Where(c => c.IsActive)
                .Where(c => c.IsVisibleInMenu)
                .OrderBy(c => c.DisplayOrder)
                .Select(c => new CategoryTreeResponse
                {
                    Id = c.Id,
                    Name = c.Name,
                    Slug = c.Slug,
                    ImageUrl = c.ImageUrl,
                    Icon = c.Icon,
                    DisplayOrder = c.DisplayOrder,
                    Children = BuildCategoryTree(categories, c.Id)
                })
                .ToList();
        }
    }
}
