using FluentValidation;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Sportswear.Core.Behaviors;
using System.Reflection;

namespace Sportswear.Core
{
    public static class ModuleCoreDependencies
    {
        public static IServiceCollection AddCoreDependencies(this IServiceCollection services)
        {
            //Register Configuration Of Mediator - On Assembly => dll
            services.AddMediatR(cfg => cfg.RegisterServicesFromAssemblies(Assembly.GetExecutingAssembly()));

            //Configuration Of Auto Mapper
            services.AddAutoMapper(Assembly.GetExecutingAssembly());
            //services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

            // Get Validators
            services.AddValidatorsFromAssembly(Assembly.GetExecutingAssembly());

            services.AddTransient(typeof(IPipelineBehavior<,>), typeof(ValidationBehavior<,>));

            return services;
        }
    }
}
