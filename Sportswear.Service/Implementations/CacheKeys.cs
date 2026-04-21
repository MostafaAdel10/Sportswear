namespace Sportswear.Service.Implementations
{
    public static class CacheKeys
    {
        // Categories
        public const string Categories = "categories";
        public const string CategoryById = "category_{0}";

        // Brands
        public const string Brands = "brands";
        public const string BrandById = "brand_{0}";

        // Products
        public const string ProductsList = "products_list";
        public const string ProductById = "product_{0}";
        public const string ProductFullDetails = "product_full_{0}";
        public const string ProductWithVariants = "product_variants_{0}";
    }
}
