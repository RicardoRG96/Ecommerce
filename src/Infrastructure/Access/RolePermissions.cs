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
                Permissions.Users.Update,
                Permissions.Users.UpdatePassword,

                Permissions.Products.Read,
                Permissions.Products.Publish,
                Permissions.Products.Unpublish,
                Permissions.Products.ReadAllProducts,

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
                Permissions.Users.Update,
                Permissions.Users.UpdatePassword,

                Permissions.Brands.Read,
                Permissions.Brands.Create,
                Permissions.Brands.Update,

                Permissions.Categories.Read,
                Permissions.Categories.Create,
                Permissions.Categories.Update,

                Permissions.Products.Read,
                Permissions.Products.Create,
                Permissions.Products.Update,
                Permissions.Products.AssignBrand,
                Permissions.Products.AssignCategory,
                Permissions.Products.ReadAllProducts,
                Permissions.Products.IndexProductsToSearch,

                Permissions.ProductGalleries.AddMedia,
                Permissions.ProductGalleries.RemoveMedia,
                Permissions.ProductGalleries.Reorder,

                Permissions.ProductSkus.Create,
                Permissions.ProductSkus.Update,
                Permissions.ProductSkuStocks.AdjustStock,
                Permissions.ProductSkuStocks.ReserveStock,
                Permissions.ProductSkuStocks.ReleaseStock,
                Permissions.ProductSkus.UpdatePrice,

                Permissions.Attributes.Create,
                Permissions.Attributes.Update,
                Permissions.AttributeValues.Create,
                Permissions.AttributeValues.Update,
                Permissions.ProductAttributeValues.AssignToSku,
                Permissions.ProductAttributeValues.UnassignFromSku,
                Permissions.ProductAttributeValues.Read
            ],

            [Roles.SalesManager] =
            [
                Permissions.Users.Update,
                Permissions.Users.UpdatePassword,

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
                Permissions.Users.Update,
                Permissions.Users.UpdatePassword,
                Permissions.Users.Read,
                Permissions.Orders.Read,
                Permissions.Orders.ViewAll,
                Permissions.PaymentDetails.Read
            ],

            [Roles.Customer] =
            [
                Permissions.Users.Update,
                Permissions.Users.UpdatePassword,

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
                Permissions.Addresses.Delete,
                Permissions.Addresses.AssignDefaultAddress
            ],

            [Roles.Guest] =
            [
                Permissions.Products.Read
            ]
        };
    }
}
