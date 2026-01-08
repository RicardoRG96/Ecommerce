namespace Infrastructure.Access
{
    public sealed class RolePermissions
    {
        public static readonly Dictionary<string, IReadOnlyCollection<string>> Map =
        new()
        {
            [Roles.Admin] = PermissionHelper.GetAllPermissions().ToList(),

            [Roles.PlatformManager] =
            [
                Permissions.Users.Read,
                Permissions.Users.AssignRole,

                Permissions.Products.Read,
                Permissions.Products.Publish,
                Permissions.Products.Unpublish,

                Permissions.Discounts.Read,
                Permissions.Discounts.Activate,
                Permissions.Discounts.Deactivate,

                Permissions.Orders.Read,
                Permissions.Orders.ViewAll,
                Permissions.Orders.UpdateStatus,

                Permissions.PaymentDetails.Read,
                Permissions.PaymentDetails.ViewAll
            ],

            [Roles.CatalogManager] =
            [
                Permissions.Brands.Read,
                Permissions.Brands.Create,
                Permissions.Brands.Update,

                Permissions.Categories.Read,
                Permissions.Categories.Create,
                Permissions.Categories.Update,

                Permissions.SubCategories.Read,
                Permissions.SubCategories.Create,
                Permissions.SubCategories.Update,
                Permissions.SubCategories.AssignToCategory,

                Permissions.Products.Read,
                Permissions.Products.Create,
                Permissions.Products.Update,
                Permissions.Products.AssignBrand,
                Permissions.Products.AssignCategory,

                Permissions.ProductGalleries.Add,
                Permissions.ProductGalleries.Remove,
                Permissions.ProductGalleries.Reorder,

                Permissions.ProductSkus.Create,
                Permissions.ProductSkus.Update,
                Permissions.ProductSkus.UpdateStock,
                Permissions.ProductSkus.UpdatePrice,

                Permissions.ProductAttributes.Create,
                Permissions.ProductAttributes.AssignToSku
            ],

            [Roles.SalesManager] =
            [
                Permissions.Orders.Read,
                Permissions.Orders.ViewAll,
                Permissions.Orders.UpdateStatus,
                Permissions.Orders.Cancel,

                Permissions.PaymentDetails.Read,
                Permissions.PaymentDetails.ViewAll,
                Permissions.PaymentDetails.Confirm,
                Permissions.PaymentDetails.Refund
            ],

            [Roles.CustomerSupport] =
            [
                Permissions.Users.Read,
                Permissions.Orders.Read,
                Permissions.Orders.ViewAll,
                Permissions.PaymentDetails.Read
            ],

            [Roles.Customer] =
            [
                Permissions.Products.Read,

                Permissions.Wishlists.Read,
                Permissions.Wishlists.Create,
                Permissions.Wishlists.AddItem,
                Permissions.Wishlists.RemoveItem,

                Permissions.Carts.Read,
                Permissions.Carts.Create,
                Permissions.Carts.Update,
                Permissions.Carts.Checkout,

                Permissions.CartItems.Create,
                Permissions.CartItems.Update,
                Permissions.CartItems.Delete,

                Permissions.Orders.Read,
                Permissions.Orders.Create,
                Permissions.Orders.Cancel,

                Permissions.PaymentDetails.Create,
                Permissions.PaymentDetails.Read,

                Permissions.Addresses.Read,
                Permissions.Addresses.Create,
                Permissions.Addresses.Update,
                Permissions.Addresses.Delete
            ],

            [Roles.Guest] =
            [
                Permissions.Products.Read
            ]
        };
    }
}
