using EntityFrameworkCore.EncryptColumn.Extension;
using EntityFrameworkCore.EncryptColumn.Interfaces;
using EntityFrameworkCore.EncryptColumn.Util;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Sportswear.DataAccess.Entities;
using Sportswear.DataAccess.Entities.Identity;
using System.Reflection;

namespace Sportswear.Infrastructure.Context
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser, Role, int>
    {
        private readonly IEncryptionProvider _encryptionProvider;

        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
            _encryptionProvider = new GenerateEncryptionProvider("43b47fd3d77c4323bebf3484334e8a5a");
        }

        //DbSets

        // Catalog
        public DbSet<Product> Products { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<Brand> Brands { get; set; }
        public DbSet<ProductImage> ProductImages { get; set; }
        public DbSet<ProductVariant> ProductVariants { get; set; }

        // Cart & Orders
        public DbSet<Cart> Carts { get; set; }
        public DbSet<CartItem> CartItems { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<OrderItem> OrderItems { get; set; }
        public DbSet<Payment> Payments { get; set; }
        public DbSet<Shipment> Shipments { get; set; }
        public DbSet<ShippingMethod> ShippingMethods { get; set; }


        // Marketing
        public DbSet<Discount> Discounts { get; set; }
        public DbSet<Product_Discount> Product_Discounts { get; set; }
        public DbSet<Review> Reviews { get; set; }

        // User Management
        public DbSet<UserRefreshToken> UserRefreshTokens { get; set; }

        // Attribute Templates
        public DbSet<ProductAttributeTemplate> ProductAttributeTemplates { get; set; }
        public DbSet<ProductVariantAttribute> ProductVariantAttributes { get; set; }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
            modelBuilder.UseEncryption(_encryptionProvider);

        }
    }
}
