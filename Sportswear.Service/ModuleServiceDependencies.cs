using Microsoft.Extensions.DependencyInjection;
using Sportswear.Service.Abstract;
using Sportswear.Service.AuthServices.Implementations;
using Sportswear.Service.AuthServices.Interfaces;
using Sportswear.Service.Implementations;

namespace Sportswear.Service
{
    public static class ModuleServiceDependencies
    {
        public static IServiceCollection AddServiceDependencies(
            this IServiceCollection services)
        //,IHostEnvironment environment) // ← أضفنا environment
        {
            services.AddScoped<ICurrentUserService, CurrentUserService>();
            services.AddScoped<IProductService, ProductService>();
            services.AddScoped<IBrandService, BrandService>();
            services.AddScoped<ICategoryService, CategoryService>();
            services.AddScoped<IProduct_DiscountService, Product_DiscountService>();
            services.AddScoped<IDiscountService, DiscountService>();
            services.AddScoped<IProductVariantService, ProductVariantService>();
            services.AddScoped<IProductImageService, ProductImageService>();
            services.AddScoped<IReviewService, ReviewService>();
            services.AddScoped<IEmailsService, EmailsService>();
            services.AddScoped<IApplicationUserService, ApplicationUserService>();
            services.AddScoped<IAuthenticationService, AuthenticationService>();
            services.AddScoped<IAuthorizationService, AuthorizationService>();
            services.AddScoped<ICartItemService, CartItemService>();
            services.AddScoped<ICartService, CartService>();
            services.AddScoped<IShippingMethodService, ShippingMethodService>();
            services.AddScoped<IShipmentService, ShipmentService>();
            services.AddScoped<IOrderItemService, OrderItemService>();
            services.AddScoped<IPaymentService, PaymentService>();
            services.AddScoped<IOrderService, OrderService>();
            services.AddScoped<ISkuGeneratorService, SkuGeneratorService>();
            services.AddScoped<IFileService, FileService>();

            //// FileService — Local في Development، Azure Blob في Production
            //if (environment.IsProduction())
            //    services.AddScoped<IFileService, AzureBlobFileService>();
            //else
            //    services.AddScoped<IFileService, FileService>();

            return services;
        }
    }
}