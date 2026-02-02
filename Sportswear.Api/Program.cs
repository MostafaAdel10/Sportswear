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

// ====================== Logging ======================
Log.Logger = new LoggerConfiguration()
    .ReadFrom.Configuration(builder.Configuration)
    .CreateLogger();

builder.Host.UseSerilog();

// ====================== Services ======================
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "Sportswear API", Version = "v1" });
    c.AddSecurityDefinition(JwtBearerDefaults.AuthenticationScheme,
        new OpenApiSecurityScheme
        {
            Name = "Authorization",
            Type = SecuritySchemeType.ApiKey,
            Scheme = "Bearer",
            In = ParameterLocation.Header,
            Description = "Bearer {token}"
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

// ====================== DbContext ======================
builder.Services.AddDbContext<ApplicationDbContext>(options =>
{
    options.UseSqlServer(builder.Configuration.GetConnectionString("cs"));
});

// ====================== Identity ======================
builder.Services.AddIdentity<ApplicationUser, Role>(options =>
{
    options.Password.RequiredLength = 6;
    options.Password.RequireDigit = true;
    options.Password.RequireUppercase = true;
    options.Password.RequireLowercase = true;

    options.SignIn.RequireConfirmedEmail = true;
})
.AddEntityFrameworkStores<ApplicationDbContext>()
.AddDefaultTokenProviders();

// ====================== JWT ======================
var jwtSettings = new JwtSettings();
builder.Configuration.GetSection("jwtSettings").Bind(jwtSettings);
builder.Services.AddSingleton(jwtSettings);

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
.AddJwtBearer(options =>
{
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = jwtSettings.ValidateIssuer,
        ValidIssuer = jwtSettings.Issuer,

        ValidateAudience = jwtSettings.ValidateAudience,
        ValidAudience = jwtSettings.Audience,
        ValidateLifetime = jwtSettings.ValidateLifeTime,
        ValidateIssuerSigningKey = jwtSettings.ValidateIssuerSigningKey,

        IssuerSigningKey = new SymmetricSecurityKey(
            Encoding.UTF8.GetBytes(jwtSettings.Secret))
    };
});


// ====================== Authorization Policies ======================
builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("CreateProduct",
        p => p.RequireClaim("Create Product", "True"));
    options.AddPolicy("EditProduct",
        p => p.RequireClaim("Edit Product", "True"));
    options.AddPolicy("DeleteProduct",
        p => p.RequireClaim("Delete Product", "True"));
});

// ====================== CORS ======================
const string CORS = "_cors";
var corsSettings = builder.Configuration.GetSection("Cors");

builder.Services.AddCors(options =>
{
    options.AddPolicy(CORS, policy =>
    {
        if (corsSettings.GetValue<bool>("AllowAll"))
        {
            policy.AllowAnyOrigin()
                  .AllowAnyHeader()
                  .AllowAnyMethod();
        }
    });
});


// ====================== EmailSettings ======================
var emailSettings = new EmailSettings();
builder.Configuration.GetSection("emailSettings").Bind(emailSettings);
builder.Services.AddSingleton(emailSettings);

// ====================== IUrlHelper ======================
builder.Services.AddSingleton<IActionContextAccessor, ActionContextAccessor>();
builder.Services.AddTransient<IUrlHelper>(sp =>
{
    var actionContext = sp.GetRequiredService<IActionContextAccessor>().ActionContext;
    var factory = sp.GetRequiredService<IUrlHelperFactory>();
    return factory.GetUrlHelper(actionContext);
});


// ====================== Localization ======================
builder.Services.AddLocalization();
builder.Services.Configure<RequestLocalizationOptions>(options =>
{
    var cultures = new[]
    {
        new CultureInfo("en-US"),
        new CultureInfo("ar-EG")
    };

    options.DefaultRequestCulture = new RequestCulture("en-US");
    options.SupportedCultures = cultures;
    options.SupportedUICultures = cultures;
});

// ====================== DI Layers ======================
builder.Services
    .AddInfrastructureDependencies()
    .AddServiceDependencies()
    .AddCoreDependencies();

// ====================== File Upload ======================
builder.Services.Configure<FileUploadOptions>(
    builder.Configuration.GetSection("FileUpload"));

// ====================== Build ======================
var app = builder.Build();

// ====================== Seed ======================
using (var scope = app.Services.CreateScope())
{
    var userManager = scope.ServiceProvider.GetRequiredService<UserManager<ApplicationUser>>();
    var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<Role>>();

    await RoleSeeder.SeedAsync(roleManager);
    await UserSeeder.SeedAsync(userManager);
}

// ====================== Middleware ======================
app.UseMiddleware<ErrorHandlerMiddleware>();

app.UseSerilogRequestLogging();

app.UseSwagger();
app.UseSwaggerUI();

app.UseRequestLocalization(
    app.Services.GetRequiredService<IOptions<RequestLocalizationOptions>>().Value);

app.UseHttpsRedirection();

app.UseStaticFiles();
app.UseRouting();

app.UseCors(CORS);

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();