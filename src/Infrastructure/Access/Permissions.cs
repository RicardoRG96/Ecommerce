namespace Infrastructure.Access
{
    public static class Permissions
    {
        // -------------------------
        // USER & ADDRESS
        // -------------------------

        public static class Users
        {
            public const string Read = "User.Read";
            public const string Create = "User.Create";
            public const string Update = "User.Update";
            public const string Delete = "User.Delete";
            public const string AssignRole = "User.AssignRole";
            public const string AssignPermission = "User.AssignPermission";
            public const string UnassignRole = "User.UnassignRole";
            public const string UnassignPermission = "User.UnassignPermission";
            public const string UpdatePassword = "User.UpdatePassword";
        }

        public static class Roles
        {
            public const string Create = "Role.Create";
            public const string Read = "Role.Read";
            public const string Update = "Role.Update";
            public const string Delete = "Role.Delete";
            public const string AssignPermission = "Role.AssignPermission";
            public const string UnassignPermission = "Role.UnassignPermission";
        }

        public static class Addresses
        {
            public const string Read = "Address.Read";
            public const string Create = "Address.Create";
            public const string Update = "Address.Update";
            public const string Delete = "Address.Delete";
            public const string AssignDefaultAddress = "Address.AssignDefaultAddress";
        }

        public static class AddressUsers
        {
            public const string Assign = "AddressUser.Assign";
            public const string Unassign = "AddressUser.Unassign";
        }

        // -------------------------
        // GEOGRAPHY
        // -------------------------

        public static class Countries
        {
            public const string Read = "Country.Read";
            public const string Create = "Country.Create";
            public const string Update = "Country.Update";
            public const string Delete = "Country.Delete";
        }

        public static class Regions
        {
            public const string Read = "Region.Read";
            public const string Create = "Region.Create";
            public const string Update = "Region.Update";
            public const string Delete = "Region.Delete";
        }

        public static class Municipalities
        {
            public const string Read = "Municipality.Read";
            public const string Create = "Municipality.Create";
            public const string Update = "Municipality.Update";
            public const string Delete = "Municipality.Delete";
        }

        // -------------------------
        // CATALOG
        // -------------------------

        public static class Brands
        {
            public const string Read = "Brand.Read";
            public const string Create = "Brand.Create";
            public const string Update = "Brand.Update";
            public const string Delete = "Brand.Delete";
            public const string Activate = "Brand.Activate";
            public const string Deactivate = "Brand.Deactivate";
        }

        public static class Categories
        {
            public const string Read = "Category.Read";
            public const string Create = "Category.Create";
            public const string Update = "Category.Update";
            public const string Delete = "Category.Delete";
            public const string Activate = "Category.Activate";
            public const string Deactivate = "Category.Deactivate";
            public const string ChangeCategoryParent = "Category.ChangeCategoryParent";
        }

        public static class Products
        {
            public const string Read = "Product.Read";
            public const string Create = "Product.Create";
            public const string Update = "Product.Update";
            public const string Delete = "Product.Delete";
            public const string Publish = "Product.Publish";
            public const string Unpublish = "Product.Unpublish";
            public const string AssignCategory = "Product.AssignCategory";
            public const string AssignBrand = "Product.AssignBrand";
            public const string ReadAllProducts = "Product.ReadAllProducts";
            public const string Activate = "Product.Activate";
            public const string Deactivate = "Product.Deactivate";
            public const string IndexProductsToSearch = "Product.IndexProductsToSearch";
        }

        public static class ProductGalleries
        {
            public const string Read = "ProductGallery.Read";
            public const string AddMedia = "ProductGallery.AddMedia";
            public const string RemoveMedia = "ProductGallery.RemoveMedia";
            public const string SetPrimaryMedia = "ProductGallery.SetPrimaryMedia";
            public const string Reorder = "ProductGallery.Reorder";
        }

        public static class ProductSkus
        {
            public const string Read = "ProductSku.Read";
            public const string Create = "ProductSku.Create";
            public const string Update = "ProductSku.Update";
            public const string Delete = "ProductSku.Delete";
            public const string Activate = "ProductSku.Activate";
            public const string Deactivate = "ProductSku.Deactivate";
            public const string UpdatePrice = "ProductSku.UpdatePrice";
        }

        public static class Attributes
        {
            public const string Read = "Attribute.Read";
            public const string Create = "Attribute.Create";
            public const string Update = "Attribute.Update";
            public const string Delete = "Attribute.Delete";
            public const string Activate = "Attribute.Activate";
            public const string Deactivate = "Attribute.Deactivate";
        }

        public static class AttributeValues
        {
            public const string Read = "AttributeValue.Read";
            public const string Create = "AttributeValue.Create";
            public const string Update = "AttributeValue.Update";
            public const string Delete = "AttributeValue.Delete";
            public const string Activate = "AttributeValue.Activate";
            public const string Deactivate = "AttributeValue.Deactivate";
        }

        public static class ProductAttributeValues
        {
            public const string Read = "ProductAttributeValue.Read";
            public const string AssignToSku = "ProductAttributeValue.AssignToSku";
            public const string UnassignFromSku = "ProductAttributeValue.UnassignFromSku";
        }

        // -------------------------
        // INVENTORY
        // -------------------------

        public static class Warehouse
        {
            public const string Read = "Warehouse.Read";
            public const string Create = "Warehouse.Create";
            public const string Update = "Warehouse.Update";
            public const string Delete = "Warehouse.Delete";
            public const string SetDefaultWarehouse = "Warehouse.SetDefault";
        }

        public static class ProductSkuStocks
        {
            public const string AdjustStock = "ProductSkuStock.AdjustStock";
            public const string ReserveStock = "ProductSkuStock.ReserveStock";
            public const string ReleaseStock = "ProductSkuStock.ReleaseStock";
            public const string Create = "ProductSkuStock.Create";
            public const string Read = "ProductSkuStock.Read";
            public const string Update = "ProductSkuStock.Update";
            public const string Delete = "ProductSkuStock.Delete";
            public const string Activate = "ProductSkuStock.Activate";
            public const string Deactivate = "ProductSkuStock.Deactivate";
            public const string SetWarehouse = "ProductSkuStock.SetWarehouse";
        }

        // -------------------------
        // DISCOUNTS
        // -------------------------

        public static class Discounts
        {
            public const string Read = "Discount.Read";
            public const string Create = "Discount.Create";
            public const string Update = "Discount.Update";
            public const string Delete = "Discount.Delete";
            public const string Activate = "Discount.Activate";
            public const string Deactivate = "Discount.Deactivate";
            public const string Assign = "Discount.Assign";
            public const string Unassign = "Discount.Unassign";
        }

        public static class DiscountCodes
        {
            public const string Read = "DiscountCode.Read";
            public const string Create = "DiscountCode.Create";
            public const string Update = "DiscountCode.Update";
            public const string Delete = "DiscountCode.Delete";
            public const string Assign = "DiscountCode.Assign";
            public const string Unassign = "DiscountCode.Unassign";
            public const string Apply = "DiscountCode.Apply";
            public const string Activate = "DiscountCode.Activate";
            public const string Deactivate = "DiscountCode.Deactivate";
            public const string Validate = "DiscountCode.Validate";
        }

        // -------------------------
        // SHOPPING
        // -------------------------

        public static class Wishlists
        {
            public const string Read = "Wishlist.Read";
            public const string Create = "Wishlist.Create";
            public const string Delete = "Wishlist.Delete";
            public const string AddItem = "Wishlist.AddItem";
            public const string RemoveItem = "Wishlist.RemoveItem";
        }

        public static class Carts
        {
            public const string Read = "Cart.Read";
            public const string Create = "Cart.Create";
            public const string Update = "Cart.Update";
            public const string Delete = "Cart.Delete";
            public const string Checkout = "Cart.Checkout";
        }

        public static class CartItems
        {
            public const string Read = "CartItem.Read";
            public const string Create = "CartItem.Create";
            public const string Update = "CartItem.Update";
            public const string Delete = "CartItem.Delete";
        }

        // -------------------------
        // ORDERS & PAYMENTS
        // -------------------------

        public static class Orders
        {
            public const string Read = "Order.Read";
            public const string Create = "Order.Create";
            public const string Cancel = "Order.Cancel";
            public const string UpdateStatus = "Order.UpdateStatus";
            public const string ViewAll = "Order.ViewAll";
        }

        public static class OrderItems
        {
            public const string Read = "OrderItem.Read";
            public const string Update = "OrderItem.Update";
            public const string Delete = "OrderItem.Delete";
        }

        public static class PaymentDetails
        {
            public const string Read = "PaymentDetail.Read";
            public const string Create = "PaymentDetail.Create";
            public const string Confirm = "PaymentDetail.Confirm";
            public const string Refund = "PaymentDetail.Refund";
            public const string ViewAll = "PaymentDetail.ViewAll";
        }

        // -------------------------
        // TAXES
        // -------------------------

        public static class ProductTaxCategories
        {
            public const string Read = "ProductTaxCategory.Read";
            public const string Create = "ProductTaxCategory.Create";
            public const string Update = "ProductTaxCategory.Update";
            public const string Delete = "ProductTaxCategory.Delete";
            public const string Activate = "ProductTaxCategory.Activate";
            public const string Deactivate = "ProductTaxCategory.Deactivate";
            public const string Assign = "ProductTaxCategory.Assign";
            public const string Unassign = "ProductTaxCategory.Unassign";
        }

        public static class TaxRates
        {
            public const string Read = "TaxRate.Read";
            public const string Create = "TaxRate.Create";
            public const string Update = "TaxRate.Update";
            public const string Delete = "TaxRate.Delete";
            public const string Activate = "TaxRate.Activate";
            public const string Deactivate = "TaxRate.Deactivate";
        }
    }
}
