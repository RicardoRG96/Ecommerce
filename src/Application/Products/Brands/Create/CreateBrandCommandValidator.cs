using FluentValidation;

namespace Application.Products.Brands.Create
{
    public sealed class CreateBrandCommandValidator : AbstractValidator<CreateBrandCommand>
    {
        public CreateBrandCommandValidator()
        {
            RuleFor(c => c.Name).NotEmpty().MaximumLength(150);
            RuleFor(c => c.Description).MaximumLength(500);
            RuleFor(c => c.LogoUrl).MaximumLength(500);
            RuleFor(c => c.BannerUrl).MaximumLength(500);
            RuleFor(c => c.WebsiteUrl).MaximumLength(300);
            RuleFor(c => c.IsActive).NotEmpty();
            RuleFor(c => c.IsFeatured).NotEmpty();
            RuleFor(c => c.DisplayOrder).NotEmpty();
            RuleFor(c => c.MetaTitle).MaximumLength(160);
            RuleFor(c => c.MetaDescription).MaximumLength(300);
            RuleFor(c => c.MetaKeywords).MaximumLength(500);
        }
    }
}
