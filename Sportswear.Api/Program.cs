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
using Sportswear.Core;
using Sportswear.Core.Filters;
using Sportswear.Core.Middleware;
using Sportswear.DataAccess.Entities.Identity;
using Sportswear.DataAccess.Helpers;
using Sportswear.Infrastructure;
using Sportswear.Infrastructure.Context;
using Sportswear.Infrastructure.Seeder;
using Sportswear.Service;
using Sportswear.Service.Implementations;
using System.Globalization;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();


// For Swagger/OpenAPI support

// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
//builder.Services.AddOpenApi();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();


//AddDbContext
builder.Services.AddDbContext<ApplicationDbContext>(options =>
{
    options.UseSqlServer(builder.Configuration.GetConnectionString("cs"));
});


//Service Add Identity
builder.Services.AddIdentity<ApplicationUser, Role>(options =>
{
    options.SignIn.RequireConfirmedAccount = true;
    options.Password.RequireNonAlphanumeric = true;
    options.Password.RequireDigit = true;
    options.Password.RequireLowercase = true;
    options.Password.RequireUppercase = true;
    options.Password.RequiredLength = 6;
    options.Password.RequiredUniqueChars = 1;

    // Lockout settings.
    options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(5);
    options.Lockout.MaxFailedAccessAttempts = 5;
    options.Lockout.AllowedForNewUsers = true;

    // User settings.
    options.User.AllowedUserNameCharacters =
    "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789-._@+";
    options.User.RequireUniqueEmail = true;
    options.SignIn.RequireConfirmedEmail = true;

}).AddEntityFrameworkStores<ApplicationDbContext>().AddDefaultTokenProviders();


//JWT Authentication
var jwtSettings = new JwtSettings();
var emailSettings = new EmailSettings();
builder.Configuration.GetSection(nameof(jwtSettings)).Bind(jwtSettings);
builder.Configuration.GetSection(nameof(emailSettings)).Bind(emailSettings);

builder.Services.AddSingleton(jwtSettings);
builder.Services.AddSingleton(emailSettings);

builder.Services.AddAuthentication(x =>
{
    x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(x =>
{
    x.RequireHttpsMetadata = false;
    x.SaveToken = true;
    x.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = jwtSettings.ValidateIssuer,
        ValidIssuers = new[] { jwtSettings.Issuer },
        ValidateIssuerSigningKey = jwtSettings.ValidateIssuerSigningKey,
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(jwtSettings.Secret)),
        ValidAudience = jwtSettings.Audience,
        ValidateAudience = jwtSettings.ValidateAudience,
        ValidateLifetime = jwtSettings.ValidateLifeTime,
    };
});


//Swagger Gn
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "Sportswear Project", Version = "v1" });
    c.EnableAnnotations();

    c.AddSecurityDefinition(JwtBearerDefaults.AuthenticationScheme, new OpenApiSecurityScheme
    {
        Description = "JWT Authorization header using the Bearer scheme (Example: 'Bearer 12345abcdef')",
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


//Authorize Based On Claim (Policy)
builder.Services.AddAuthorization(option =>
{
    option.AddPolicy("CreateProduct", policy =>
    {
        policy.RequireClaim("Create Product", "True");
    });
    option.AddPolicy("DeleteProduct", policy =>
    {
        policy.RequireClaim("Delete Product", "True");
    });
    option.AddPolicy("EditProduct", policy =>
    {
        policy.RequireClaim("Edit Product", "True");
    });
});


//-------------------------------
//Registration

#region Dependency Injections
builder.Services.AddInfrastructureDependencies()
                .AddServiceDependencies()
                .AddCoreDependencies();
#endregion

#region Localization
builder.Services.AddControllersWithViews();
builder.Services.AddLocalization(opt =>
{
    opt.ResourcesPath = "";
});

builder.Services.Configure<RequestLocalizationOptions>(options =>
{
    List<CultureInfo> supportedCultures = new List<CultureInfo>
    {
            new CultureInfo("en-US"),
            new CultureInfo("de-DE"),
            new CultureInfo("fr-FR"),
            new CultureInfo("ar-EG")
    };

    options.DefaultRequestCulture = new RequestCulture("en-US");
    options.SupportedCultures = supportedCultures;
    options.SupportedUICultures = supportedCultures;
});

#endregion

#region AllowCORS
var CORS = "_cors";
builder.Services.AddCors(options =>
{
    options.AddPolicy(name: CORS,
                      policy =>
                      {
                          policy.AllowAnyHeader();
                          policy.AllowAnyMethod();
                          policy.AllowAnyOrigin();
                      });
});
#endregion


//IUrlHelper and IActionContextAccessor
builder.Services.AddSingleton<IActionContextAccessor, ActionContextAccessor>();
builder.Services.AddTransient<IUrlHelper>(x =>
{
    var actionContext = x.GetRequiredService<IActionContextAccessor>().ActionContext;
    var factory = x.GetRequiredService<IUrlHelperFactory>();
    return factory.GetUrlHelper(actionContext);
});

// File Upload Options
builder.Services.Configure<FileUploadOptions>(builder.Configuration.GetSection("FileUpload"));

//Authentication Filter
builder.Services.AddScoped<AuthFilter>();

//for cash memory
//builder.Services.AddMemoryCache();

//Serilog
Log.Logger = new LoggerConfiguration()
              .ReadFrom.Configuration(builder.Configuration).CreateLogger();
builder.Services.AddSerilog();
//---------------------------------------------------------------------------------------------

var app = builder.Build();


//Seeding Data
using (var scope = app.Services.CreateScope())
{
    var userManager = scope.ServiceProvider.GetRequiredService<UserManager<ApplicationUser>>();
    var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<Role>>();
    await RoleSeeder.SeedAsync(roleManager);
    await UserSeeder.SeedAsync(userManager);
}

app.UseMiddleware<ErrorHandlerMiddleware>();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    //app.MapOpenApi();

    app.UseSwagger();
    app.UseSwaggerUI();
}

#region Localization Middleware
var options = app.Services.GetService<IOptions<RequestLocalizationOptions>>();
app.UseRequestLocalization(options.Value);
#endregion

app.UseHttpsRedirection();

app.UseCors(CORS);
app.UseStaticFiles();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
