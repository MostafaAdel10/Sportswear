using MassTransit;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Sportswear.Service.Abstract;
using Sportswear.Service.AuthServices.Implementations;
using Sportswear.Service.AuthServices.Interfaces;
using Sportswear.Service.Consumers;
using Sportswear.Service.Implementations;

namespace Sportswear.Service
{
    public static class ModuleServiceDependencies
    {
        public static IServiceCollection AddServiceDependencies(
            this IServiceCollection services, IConfiguration configuration, IHostEnvironment environment)
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
            services.AddMemoryCache();
            services.AddScoped<ICacheService, CacheService>();
            services.AddScoped<IPosSaleService, PosSaleService>();

            // ✅ MassTransit + RabbitMQ
            services.AddMassTransit(x =>
            {
                x.AddConsumer<OrderCreatedConsumer>();
                x.AddConsumer<OrderCancelledConsumer>();
                x.AddConsumer<PosSaleCreatedConsumer>();

                if (environment.IsProduction())
                {
                    // ✅ Azure Service Bus في Production
                    x.UsingAzureServiceBus((context, cfg) =>
                    {
                        cfg.Host(configuration["AzureServiceBus:ConnectionString"]);

                        cfg.UseMessageRetry(r =>
                            r.Incremental(3,
                                TimeSpan.FromSeconds(1),
                                TimeSpan.FromSeconds(2)));

                        cfg.ConfigureEndpoints(context);
                    });
                }
                else
                {
                    // ✅ RabbitMQ في Development
                    x.UsingRabbitMq((context, cfg) =>
                    {
                        cfg.Host(configuration["RabbitMQ:Host"], h =>
                        {
                            h.Username(configuration["RabbitMQ:Username"]!);
                            h.Password(configuration["RabbitMQ:Password"]!);
                        });

                        cfg.UseMessageRetry(r =>
                            r.Incremental(3,
                                TimeSpan.FromSeconds(1),
                                TimeSpan.FromSeconds(2)));

                        cfg.ConfigureEndpoints(context);
                    });
                }
            });

            // FileService — Local في Development، Azure Blob في Production
            if (environment.IsProduction())
                services.AddScoped<IFileService, AzureBlobFileService>();
            else
                services.AddScoped<IFileService, FileService>();

            return services;
        }
    }
}