namespace Sportswear.DataAccess.AppMetaData
{
    public static class Router
    {
        public const string SingleRoute = "/{id}";
        public const string root = "api";               // صغّرنا "Api" → "api"
        public const string version = "v1";             // صغّرنا "V1" → "v1"
        public const string Rule = root + "/" + version + "/";

        public static class OrderRouting
        {
            public const string Prefix = Rule + "order";
            public const string List = Prefix + "/list";
            public const string MyOrders = Prefix + "/my-orders";
            public const string GetById = Prefix + "/{id:int}";
            public const string GetByUserId = Prefix + "/user/{userId:int}";
            public const string Create = Prefix + "/create";
            public const string EditOrderStatus = Prefix + "/order-status";
            public const string EditPaymentStatus = Prefix + "/payment-status";
        }

        public static class ShippingMethodRouting
        {
            public const string Prefix = Rule + "shippingmethod";
            public const string List = Prefix + "/list";
            public const string GetById = Prefix + SingleRoute;
            public const string Create = Prefix + "/create";
            public const string Edit = Prefix + "/edit";
            public const string Delete = Prefix + SingleRoute;
        }

        public static class CartItemRouting
        {
            public const string Prefix = Rule + "cartitem";
            public const string GetById = Prefix + SingleRoute;
            public const string List = Prefix + "/list";
            public const string GetCartSummary = Prefix + "/getcartsummary";
            public const string Create = Prefix + "/create";
            public const string Edit = Prefix + "/edit";
            public const string Delete = Prefix + SingleRoute;
        }

        public static class ReviewRouting
        {
            public const string Prefix = Rule + "review";
            public const string Create = Prefix + "/create";
            public const string Edit = Prefix + "/edit";
            public const string Delete = Prefix + SingleRoute;
            public const string GetById = Prefix + SingleRoute;
            public const string GetReviewsByProductId = Prefix + "/{productId}";
            public const string Paginated = Prefix + "/paginated";
        }

        public static class ProductImageRouting
        {
            public const string Prefix = Rule + "productimage";
            public const string List = Prefix + "/list";
            public const string Paginated = Prefix + "/paginated";
            public const string GetById = Prefix + SingleRoute;
            public const string CreateProductImages = Prefix + "/createproductimages";
            public const string CreateProductImage = Prefix + "/createproductimage";
            public const string Edit = Prefix + "/editproductimage";
            public const string Delete = Prefix + "/deleteproductimage";
        }

        public static class Product_DiscountRouting
        {
            public const string Prefix = Rule + "product_discount";
            public const string List = Prefix + "/list";
            public const string Paginated = Prefix + "/getproductsbydiscountidpaginated";
            public const string GetById = Prefix + SingleRoute;
            public const string Create = Prefix + "/adddiscounttoproducts";
            public const string Edit = Prefix + "/updateproductsfordiscount";
            public const string Delete = Prefix + "/removediscountfromproducts";
        }

        public static class ProductVariantRouting
        {
            public const string Prefix = Rule + "productvariant";
            public const string List = Prefix + "/list";
            public const string Paginated = Prefix + "/paginated";
            public const string GetById = Prefix + SingleRoute;
            public const string Create = Prefix + "/create";
            public const string Edit = Prefix + "/edit";
            public const string Delete = Prefix + SingleRoute;
        }

        public static class DiscountRouting
        {
            public const string Prefix = Rule + "discount";
            public const string List = Prefix + "/list";
            public const string Paginated = Prefix + "/paginated";
            public const string GetById = Prefix + SingleRoute;
            public const string Create = Prefix + "/create";
            public const string Edit = Prefix + "/edit";
            public const string Delete = Prefix + SingleRoute;
        }

        public static class CategoryRouting
        {
            public const string Prefix = Rule + "category";
            public const string List = Prefix + "/list";
            public const string Paginated = Prefix + "/paginated";
            public const string GetById = Prefix + SingleRoute;
            public const string Create = Prefix + "/create";
            public const string Edit = Prefix + "/edit";
            public const string Delete = Prefix + SingleRoute;
        }

        public static class BrandRouting
        {
            public const string Prefix = Rule + "brand";
            public const string List = Prefix + "/list";
            public const string Paginated = Prefix + "/paginated";
            public const string GetById = Prefix + SingleRoute;
            public const string Create = Prefix + "/create";
            public const string Edit = Prefix + "/edit";
            public const string Delete = Prefix + SingleRoute;
        }

        public static class ProductRouting
        {
            public const string Prefix = Rule + "product";
            public const string List = Prefix + "/list";
            public const string Paginated = Prefix + "/paginated";
            public const string GetById = Prefix + SingleRoute;
            public const string GetByIdWithVariants = Prefix + SingleRoute + "/with-variants";
            public const string Create = Prefix + "/create";
            public const string Edit = Prefix + "/edit";
            public const string Delete = Prefix + SingleRoute;
        }

        public static class ApplicationUserRouting
        {
            public const string Prefix = Rule + "user";
            public const string Create = Prefix + "/create";
            public const string Paginated = Prefix + "/paginated";
            public const string GetByID = Prefix + SingleRoute;
            public const string Edit = Prefix + "/edit";
            public const string Delete = Prefix + SingleRoute;
            public const string ChangePassword = Prefix + "/change-password";
        }

        public static class Authentication
        {
            public const string Prefix = Rule + "authentication";
            public const string SignIn = Prefix + "/signin";
            public const string RefreshToken = Prefix + "/refresh-token";
            public const string ValidateToken = Prefix + "/validate-token";
            public const string ConfirmEmail = "/api/authentication/confirmemail";  // غيرناها كمان
            public const string SendResetPasswordCode = Prefix + "/sendresetpasswordcode";
            public const string ConfirmResetPasswordCode = Prefix + "/confirmresetpasswordcode";
            public const string ResetPassword = Prefix + "/resetpassword";
        }

        public static class AuthorizationRouting
        {
            public const string Prefix = Rule + "authorization";
            public const string Roles = Prefix + "/roles";
            public const string Claims = Prefix + "/claims";
            public const string Create = Roles + "/create";
            public const string Edit = Roles + "/edit";
            public const string Delete = Roles + "/delete/{id}";
            public const string RoleList = Roles + "/role-list";
            public const string GetRoleById = Roles + "/role-by-id/{id}";
            public const string ManageUserRoles = Roles + "/manage-user-roles/{userId}";
            public const string ManageUserClaims = Claims + "/manage-user-claims/{userId}";
            public const string UpdateUserRoles = Roles + "/update-user-roles";
            public const string UpdateUserClaims = Claims + "/update-user-claims";
        }

        public static class EmailsRoute
        {
            public const string Prefix = Rule + "emails";
            public const string SendEmail = Prefix + "/sendemail";
        }
    }
}