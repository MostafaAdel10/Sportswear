using Hangfire;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Localization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.AspNetCore.Mvc.Routing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Serilog;
using Sportswear.Api.Filters;
using Sportswear.Core;
using Sportswear.Core.Filters;
using Sportswear.Core.Middleware;
using Sportswear.DataAccess.Entities.Identity;
using Sportswear.DataAccess.Helpers;
using Sportswear.Infrastructure;
using Sportswear.Infrastructure.Context;
using Sportswear.Infrastructure.Seeder;
using Sportswear.Service;
using Sportswear.Service.Abstract;
using Sportswear.Service.Implementations;
using System.Globalization;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

#region Controllers & Swagger

// Add controllers
builder.Services.AddControllers();

// Add Swagger / OpenAPI support
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

#endregion

#region Database Configuration

// Configure SQL Server DbContext
builder.Services.AddDbContext<ApplicationDbContext>(options =>
{
    options.UseSqlServer(builder.Configuration.GetConnectionString("cs"));
});

#endregion

#region Identity Configuration

// Configure ASP.NET Identity settings
builder.Services.AddIdentity<ApplicationUser, Role>(options =>
{
    // Password settings
    options.Password.RequireNonAlphanumeric = true;
    options.Password.RequireDigit = true;
    options.Password.RequireLowercase = true;
    options.Password.RequireUppercase = true;
    options.Password.RequiredLength = 6;

    // Lockout settings
    options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(5);
    options.Lockout.MaxFailedAccessAttempts = 5;

    // User settings
    options.User.RequireUniqueEmail = true;
    options.SignIn.RequireConfirmedEmail = true;

}).AddEntityFrameworkStores<ApplicationDbContext>()
  .AddDefaultTokenProviders();

#endregion

#region JWT Authentication

var jwtSettings = new JwtSettings();
var emailSettings = new EmailSettings();

// Bind settings from configuration
builder.Configuration.GetSection(nameof(jwtSettings)).Bind(jwtSettings);
builder.Configuration.GetSection(nameof(emailSettings)).Bind(emailSettings);

// Register as singleton
builder.Services.AddSingleton(jwtSettings);
builder.Services.AddSingleton(emailSettings);

// Configure JWT Authentication
builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(options =>
{
    options.RequireHttpsMetadata = false;
    options.SaveToken = true;

    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = jwtSettings.ValidateIssuer,
        ValidIssuers = new[] { jwtSettings.Issuer },

        ValidateIssuerSigningKey = jwtSettings.ValidateIssuerSigningKey,
        IssuerSigningKey = new SymmetricSecurityKey(
            Encoding.ASCII.GetBytes(jwtSettings.Secret)),

        ValidateAudience = jwtSettings.ValidateAudience,
        ValidAudience = jwtSettings.Audience,

        ValidateLifetime = jwtSettings.ValidateLifeTime
    };
});

#endregion

#region Swagger Configuration

builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "Sportswear Project",
        Version = "v1"
    });

    // Enable annotations
    c.EnableAnnotations();

    // Add JWT support to Swagger
    c.AddSecurityDefinition(JwtBearerDefaults.AuthenticationScheme,
        new OpenApiSecurityScheme
        {
            Description = "JWT Authorization using Bearer scheme",
            Name = "Authorization",
            In = ParameterLocation.Header,
            Type = SecuritySchemeType.ApiKey,
            Scheme = JwtBearerDefaults.AuthenticationScheme
        });

    c.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = JwtBearerDefaults.AuthenticationScheme
                }
            },
            Array.Empty<string>()
        }
    });
});

#endregion

#region Hangfire Configuration

// Configure Hangfire with SQL Server storage
builder.Services.AddHangfire(config =>
    config.UseSqlServerStorage(
        builder.Configuration.GetConnectionString("HangfireConnection")));

builder.Services.AddHangfireServer();

// Register background job
builder.Services.AddScoped<IDiscountCleanupJob, DiscountCleanupJob>();

#endregion

#region Authorization Policies

builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("CreateProduct",
        policy => policy.RequireClaim("Create Product", "True"));

    options.AddPolicy("DeleteProduct",
        policy => policy.RequireClaim("Delete Product", "True"));

    options.AddPolicy("EditProduct",
        policy => policy.RequireClaim("Edit Product", "True"));
});

#endregion

#region Dependency Injection (Layers)

builder.Services
    .AddInfrastructureDependencies()
    .AddServiceDependencies()
    .AddCoreDependencies();

#endregion

#region Localization

builder.Services.AddControllersWithViews();

builder.Services.AddLocalization(options =>
{
    options.ResourcesPath = "";
});

builder.Services.Configure<RequestLocalizationOptions>(options =>
{
    var supportedCultures = new List<CultureInfo>
    {
        new("en-US"),
        new("de-DE"),
        new("fr-FR"),
        new("ar-EG")
    };

    options.DefaultRequestCulture = new RequestCulture("en-US");
    options.SupportedCultures = supportedCultures;
    options.SupportedUICultures = supportedCultures;
});

#endregion

#region CORS

const string CORS = "_cors";

builder.Services.AddCors(options =>
{
    options.AddPolicy(CORS, policy =>
    {
        policy.AllowAnyHeader()
              .AllowAnyMethod()
              .AllowAnyOrigin();
    });
});

#endregion

#region Helpers & Utilities

// Register URL helper
builder.Services.AddSingleton<IActionContextAccessor, ActionContextAccessor>();

builder.Services.AddTransient<IUrlHelper>(provider =>
{
    var actionContext = provider
        .GetRequiredService<IActionContextAccessor>().ActionContext;

    var factory = provider.GetRequiredService<IUrlHelperFactory>();

    return factory.GetUrlHelper(actionContext);
});

// Access current HTTP context
builder.Services.AddHttpContextAccessor();

// File upload configuration
builder.Services.Configure<FileUploadOptions>(
    builder.Configuration.GetSection("FileUpload"));

// Custom authentication filter
builder.Services.AddScoped<AuthFilter>();

#endregion

#region Logging (Serilog)

Log.Logger = new LoggerConfiguration()
    .ReadFrom.Configuration(builder.Configuration)
    .CreateLogger();

builder.Services.AddSerilog();

#endregion

// =============================================

var app = builder.Build();

#region Database Seeding

using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();

    // Apply migrations automatically
    await db.Database.MigrateAsync();

    var userManager = scope.ServiceProvider.GetRequiredService<UserManager<ApplicationUser>>();
    var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<Role>>();

    await RoleSeeder.SeedAsync(roleManager);
    await UserSeeder.SeedAsync(userManager);
}

#endregion

#region Middleware Pipeline

// Global error handling middleware
app.UseMiddleware<ErrorHandlerMiddleware>();

// Swagger
app.UseSwagger();
app.UseSwaggerUI();

// Localization middleware
var localizationOptions = app.Services.GetService<IOptions<RequestLocalizationOptions>>();
app.UseRequestLocalization(localizationOptions.Value);

// HTTPS
app.UseHttpsRedirection();

// CORS
app.UseCors(CORS);

// Static files
app.UseStaticFiles();

// Authentication & Authorization
app.UseAuthentication();
app.UseAuthorization();

// Hangfire Dashboard (secured)
app.UseHangfireDashboard("/hangfire", new DashboardOptions
{
    Authorization = new[] { new HangfireAuthorizationFilter() }
});

#endregion

#region Hangfire Jobs Scheduling

using (var scope = app.Services.CreateScope())
{
    var recurringJobManager = scope.ServiceProvider
        .GetRequiredService<IRecurringJobManager>();

    // Schedule daily job to clean expired discounts
    recurringJobManager.AddOrUpdate<IDiscountCleanupJob>(
        "cleanup-expired-discounts",
        job => job.ExecuteAsync(),
        Cron.Monthly);
}

#endregion

app.MapControllers();
app.Run();