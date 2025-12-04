namespace Sportswear.DataAccess.AppMetaData
{
    public static class Router
    {
        public const string SingleRoute = "/{id}";

        public const string root = "Api";
        public const string version = "V1";
        public const string Rule = root + "/" + version + "/";

        public static class OrderRouting
        {
            public const string Prefix = Rule + "Order";
            public const string List = Prefix + "/List";
            public const string MyOrders = Prefix + "/My-Orders";
            public const string GetById = Prefix + "/{id:int}";
            public const string GetByUserId = Prefix + "/user/{userId:int}";
            public const string Create = Prefix + "/Create";
            public const string EditOrderStatus = Prefix + "/order-status";
            public const string EditPaymentStatus = Prefix + "/payment-status";
        }

        public static class ShippingMethodRouting
        {
            public const string Prefix = Rule + "ShippingMethod";
            public const string List = Prefix + "/List";
            public const string GetById = Prefix + SingleRoute;
            public const string Create = Prefix + "/Create";
            public const string Edit = Prefix + "/Edit";
            public const string Delete = Prefix + SingleRoute;
        }

        public static class CartItemRouting
        {
            public const string Prefix = Rule + "CartItem";
            public const string GetById = Prefix + SingleRoute;
            public const string List = Prefix + "/List";
            public const string GetCartSummary = Prefix + "/GetCartSummary";
            public const string Create = Prefix + "/Create";
            public const string Edit = Prefix + "/Edit";
            public const string Delete = Prefix + SingleRoute;
        }

        public static class ReviewRouting
        {
            public const string Prefix = Rule + "Review";
            public const string Create = Prefix + "/Create";
            public const string Edit = Prefix + "/Edit";
            public const string Delete = Prefix + SingleRoute;
            public const string GetById = Prefix + SingleRoute;
            public const string GetReviewsByProductId = Prefix + "/{productId}";
            public const string Paginated = Prefix + "/Paginated";
        }

        public static class ProductImageRouting
        {
            public const string Prefix = Rule + "ProductImage";
            public const string List = Prefix + "/List";
            public const string Paginated = Prefix + "/Paginated";
            public const string GetById = Prefix + SingleRoute;
            public const string CreateProductImages = Prefix + "/CreateProductImages";
            public const string CreateProductImage = Prefix + "/CreateProductImage";
            public const string Edit = Prefix + "/EditProductImage";
            public const string Delete = Prefix + "/DeleteProductImage";
        }


        public static class Product_DiscountRouting
        {
            public const string Prefix = Rule + "Product_Discount";
            public const string List = Prefix + "/List";
            public const string Paginated = Prefix + "/ GetProductsByDiscountIdPaginated";
            public const string GetById = Prefix + SingleRoute;
            public const string Create = Prefix + "/AddDiscountToProducts";
            public const string Edit = Prefix + "/UpdateProductsForDiscount";
            public const string Delete = Prefix + "/RemoveDiscountFromProducts";
        }

        public static class ProductVariantRouting
        {
            public const string Prefix = Rule + "ProductVariant";
            public const string List = Prefix + "/List";
            public const string Paginated = Prefix + "/Paginated";
            public const string GetById = Prefix + SingleRoute;
            public const string Create = Prefix + "/Create";
            public const string Edit = Prefix + "/Edit";
            public const string Delete = Prefix + SingleRoute;
        }

        public static class DiscountRouting
        {
            public const string Prefix = Rule + "Discount";
            public const string List = Prefix + "/List";
            public const string Paginated = Prefix + "/Paginated";
            public const string GetById = Prefix + SingleRoute;
            public const string Create = Prefix + "/Create";
            public const string Edit = Prefix + "/Edit";
            public const string Delete = Prefix + SingleRoute;
        }

        public static class CategoryRouting
        {
            public const string Prefix = Rule + "Category";
            public const string List = Prefix + "/List";
            public const string Paginated = Prefix + "/Paginated";
            public const string GetById = Prefix + SingleRoute;
            public const string Create = Prefix + "/Create";
            public const string Edit = Prefix + "/Edit";
            public const string Delete = Prefix + SingleRoute;
        }
        public static class BrandRouting
        {
            public const string Prefix = Rule + "Brand";
            public const string List = Prefix + "/List";
            public const string Paginated = Prefix + "/Paginated";
            public const string GetById = Prefix + SingleRoute;
            public const string Create = Prefix + "/Create";
            public const string Edit = Prefix + "/Edit";
            public const string Delete = Prefix + SingleRoute;
        }
        public static class ProductRouting
        {
            public const string Prefix = Rule + "Product";
            public const string List = Prefix + "/List";
            public const string Paginated = Prefix + "/Paginated";
            public const string GetById = Prefix + SingleRoute;
            public const string GetByIdWithVariants = Prefix + SingleRoute + "/With-Variants";
            public const string Create = Prefix + "/Create";
            public const string Edit = Prefix + "/Edit";
            public const string Delete = Prefix + SingleRoute;
        }

        public static class ApplicationUserRouting
        {
            public const string Prefix = Rule + "User";
            public const string Create = Prefix + "/Create";
            public const string Paginated = Prefix + "/Paginated";
            public const string GetByID = Prefix + SingleRoute;
            public const string Edit = Prefix + "/Edit";
            public const string Delete = Prefix + SingleRoute;
            public const string ChangePassword = Prefix + "/Change-Password";
        }

        public static class Authentication
        {
            public const string Prefix = Rule + "Authentication";
            public const string SignIn = Prefix + "/SignIn";
            public const string RefreshToken = Prefix + "/Refresh-Token";
            public const string ValidateToken = Prefix + "/Validate-Token";
            public const string ConfirmEmail = "/Api/Authentication/ConfirmEmail";
            public const string SendResetPasswordCode = Prefix + "/SendResetPasswordCode";
            public const string ConfirmResetPasswordCode = Prefix + "/ConfirmResetPasswordCode";
            public const string ResetPassword = Prefix + "/ResetPassword";

        }

        public static class AuthorizationRouting
        {
            public const string Prefix = Rule + "AuthorizationRouting";
            public const string Roles = Prefix + "/Roles";
            public const string Claims = Prefix + "/Claims";
            public const string Create = Roles + "/Create";
            public const string Edit = Roles + "/Edit";
            public const string Delete = Roles + "/Delete/{id}";
            public const string RoleList = Roles + "/Role-List";
            public const string GetRoleById = Roles + "/Role-By-Id/{id}";
            public const string ManageUserRoles = Roles + "/Manage-User-Roles/{userId}";
            public const string ManageUserClaims = Claims + "/Manage-User-Claims/{userId}";
            public const string UpdateUserRoles = Roles + "/Update-User-Roles";
            public const string UpdateUserClaims = Claims + "/Update-User-Claims";
        }

        public static class EmailsRoute
        {
            public const string Prefix = Rule + "EmailsRoute";
            public const string SendEmail = Prefix + "/SendEmail";
        }




    }
}
