using Azure.Storage.Blobs;
using Hangfire;
using HealthChecks.UI.Client;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Localization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Abstractions;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.AspNetCore.Mvc.Routing;
using Microsoft.AspNetCore.RateLimiting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Serilog;
using Serilog.Sinks.MSSqlServer;
using Sportswear.Api.Filters;
using Sportswear.Api.Helper;
using Sportswear.Core;
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
using System.Threading.RateLimiting;

var builder = WebApplication.CreateBuilder(args);

#region Controllers & Swagger
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
#endregion

#region Database Configuration
builder.Services.AddDbContext<ApplicationDbContext>(options =>
{
    options.UseSqlServer(builder.Configuration.GetConnectionString("cs"));
});
#endregion

#region Identity Configuration
builder.Services.AddIdentity<ApplicationUser, Role>(options =>
{
    options.Password.RequireNonAlphanumeric = true;
    options.Password.RequireDigit = true;
    options.Password.RequireLowercase = true;
    options.Password.RequireUppercase = true;
    options.Password.RequiredLength = 6;
    options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(5);
    options.Lockout.MaxFailedAccessAttempts = 5;
    options.User.RequireUniqueEmail = true;
    options.SignIn.RequireConfirmedEmail = true;
}).AddEntityFrameworkStores<ApplicationDbContext>()
  .AddDefaultTokenProviders();
#endregion

#region JWT Authentication
var jwtSettings = new JwtSettings();
var emailSettings = new EmailSettings();
builder.Configuration.GetSection(nameof(jwtSettings)).Bind(jwtSettings);
builder.Configuration.GetSection(nameof(emailSettings)).Bind(emailSettings);
builder.Services.AddSingleton(jwtSettings);
builder.Services.AddSingleton(emailSettings);

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
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "Sportswear Project", Version = "v1" });
    c.EnableAnnotations();
    c.AddSecurityDefinition(JwtBearerDefaults.AuthenticationScheme, new OpenApiSecurityScheme
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
builder.Services.AddHangfire(config =>
    config.UseSqlServerStorage(
        builder.Configuration.GetConnectionString("HangfireConnection")));
builder.Services.AddHangfireServer();
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
    .AddServiceDependencies(builder.Configuration, builder.Environment)
    .AddCoreDependencies();
#endregion

#region Localization
builder.Services.AddControllersWithViews();
builder.Services.AddLocalization(options => { options.ResourcesPath = ""; });
builder.Services.Configure<RequestLocalizationOptions>(options =>
{
    var supportedCultures = new List<CultureInfo>
    {
        new("en-US"), new("de-DE"), new("fr-FR"), new("ar-EG")
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
        var allowedOrigins = builder.Configuration
            .GetSection("Cors:AllowedOrigins")
            .Get<string[]>();
        policy.WithOrigins(allowedOrigins!)
              .AllowAnyHeader()
              .AllowAnyMethod()
              .AllowCredentials();
    });

    //options.AddPolicy(CORS, policy =>
    //    {
    //        policy.AllowAnyHeader();
    //        policy.AllowAnyMethod();
    //        policy.AllowAnyOrigin();
    //});
});
#endregion

#region Helpers & Utilities
builder.Services.AddSingleton<IActionContextAccessor, ActionContextAccessor>();
builder.Services.AddTransient<IUrlHelper>(provider =>
{
    var actionContext = provider
        .GetRequiredService<IActionContextAccessor>().ActionContext
        ?? new ActionContext(
            provider.GetRequiredService<IHttpContextAccessor>().HttpContext!,
            new RouteData(),
            new ActionDescriptor());

    var factory = provider.GetRequiredService<IUrlHelperFactory>();
    return factory.GetUrlHelper(actionContext);
});
builder.Services.AddHttpContextAccessor();
builder.Services.Configure<FileUploadOptions>(
    builder.Configuration.GetSection("FileUpload"));
#endregion

#region Logging (Serilog)
var connectionString = builder.Configuration.GetConnectionString("cs");
Log.Logger = new LoggerConfiguration()
    .ReadFrom.Configuration(builder.Configuration)
    .WriteTo.MSSqlServer(
        connectionString: connectionString,
        sinkOptions: new MSSqlServerSinkOptions
        {
            TableName = "SystemLogs",
            AutoCreateSqlTable = true
        })
    .CreateLogger();
builder.Services.AddSerilog();
#endregion

#region Health Checks
builder.Services.AddHealthChecks()
    .AddSqlServer(
        connectionString: builder.Configuration.GetConnectionString("cs")!,
        name: "database",
        tags: new[] { "db", "ready" })
    .AddCheck("blob-storage", () =>
    {
        try
        {
            var connStr = builder.Configuration["AzureStorage:ConnectionString"]!;
            var client = new BlobServiceClient(connStr);
            client.GetProperties();
            return HealthCheckResult.Healthy();
        }
        catch
        {
            return HealthCheckResult.Unhealthy();
        }
    }, tags: new[] { "storage", "ready" });
#endregion

#region Rate Limiting
builder.Services.AddRateLimiter(options =>
{
    // ❌ When it exceeds the limit
    options.OnRejected = async (context, cancellationToken) =>
    {
        context.HttpContext.Response.StatusCode = 429;
        await context.HttpContext.Response.WriteAsJsonAsync(new
        {
            StatusCode = 429,
            Message = "Too many requests. Please try again later."
        }, cancellationToken);
    };

    // 1. Login Policy - 5 attempts/minute
    options.AddFixedWindowLimiter(RateLimitingPolicies.Login, limiterOptions =>
    {
        limiterOptions.PermitLimit = 5;
        limiterOptions.Window = TimeSpan.FromMinutes(1);
        limiterOptions.QueueProcessingOrder = QueueProcessingOrder.OldestFirst;
        limiterOptions.QueueLimit = 0;
    });

    // 2. Policy for Register - 3 attempts/minute
    options.AddFixedWindowLimiter(RateLimitingPolicies.Register, limiterOptions =>
    {
        limiterOptions.PermitLimit = 3;
        limiterOptions.Window = TimeSpan.FromMinutes(1);
        limiterOptions.QueueProcessingOrder = QueueProcessingOrder.OldestFirst;
        limiterOptions.QueueLimit = 0;
    });

    // 3. General API Policy - 100 requests/minute
    options.AddFixedWindowLimiter(RateLimitingPolicies.Api, limiterOptions =>
    {
        limiterOptions.PermitLimit = 100;
        limiterOptions.Window = TimeSpan.FromMinutes(1);
        limiterOptions.QueueProcessingOrder = QueueProcessingOrder.OldestFirst;
        limiterOptions.QueueLimit = 0;
    });

    // 4. Upload Policy - 10 requests/minute
    options.AddFixedWindowLimiter(RateLimitingPolicies.Upload, limiterOptions =>
    {
        limiterOptions.PermitLimit = 10;
        limiterOptions.Window = TimeSpan.FromMinutes(1);
        limiterOptions.QueueProcessingOrder = QueueProcessingOrder.OldestFirst;
        limiterOptions.QueueLimit = 0;
    });

    // 5. Policy for ResetPassword - 3 attempts/minute
    options.AddFixedWindowLimiter(RateLimitingPolicies.ResetPassword, limiterOptions =>
    {
        limiterOptions.PermitLimit = 3;
        limiterOptions.Window = TimeSpan.FromMinutes(1);
        limiterOptions.QueueProcessingOrder = QueueProcessingOrder.OldestFirst;
        limiterOptions.QueueLimit = 0;
    });

    // 6. Policy for Order - 10 attempts/minute
    options.AddFixedWindowLimiter(RateLimitingPolicies.Order, limiterOptions =>
    {
        limiterOptions.PermitLimit = 10;
        limiterOptions.Window = TimeSpan.FromMinutes(1);
        limiterOptions.QueueProcessingOrder = QueueProcessingOrder.OldestFirst;
        limiterOptions.QueueLimit = 0;
    });

    // 7. Policy for Review - 5 attempts/minute
    options.AddFixedWindowLimiter(RateLimitingPolicies.Review, limiterOptions =>
    {
        limiterOptions.PermitLimit = 5;
        limiterOptions.Window = TimeSpan.FromMinutes(1);
        limiterOptions.QueueProcessingOrder = QueueProcessingOrder.OldestFirst;
        limiterOptions.QueueLimit = 0;
    });
});
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
app.UseStaticFiles();

// Static files
app.UseRateLimiter();

// Authentication & Authorization
app.UseAuthentication();
app.UseAuthorization();

// Hangfire Dashboard (secured)
app.UseHangfireDashboard("/hangfire", new DashboardOptions
{
    Authorization = new[] { new HangfireAuthorizationFilter() }
});

// ✅ Health Check Endpoints
app.MapHealthChecks("/health", new HealthCheckOptions
{
    ResponseWriter = UIResponseWriter.WriteHealthCheckUIResponse // The JSON file contains all the details.
});

app.MapHealthChecks("/health/live", new HealthCheckOptions
{
    Predicate = _ => false // But he's asking: Is the app working or not?
});

app.MapHealthChecks("/health/ready", new HealthCheckOptions
{
    Predicate = check => check.Tags.Contains("ready") // DB + Storage
});
#endregion

#region Hangfire Jobs Scheduling
using (var scope = app.Services.CreateScope())
{
    var recurringJobManager = scope.ServiceProvider
        .GetRequiredService<IRecurringJobManager>();
    recurringJobManager.AddOrUpdate<IDiscountCleanupJob>(
        "cleanup-expired-discounts",
        job => job.ExecuteAsync(),
        Cron.Monthly);
}
#endregion

app.MapControllers();
app.Run();