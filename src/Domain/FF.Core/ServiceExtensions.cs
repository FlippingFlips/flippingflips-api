using AutoMapper;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;

namespace FF.Core
{
    public static class ServiceExtensions
    {
        public static IServiceCollection AddCore(this IServiceCollection services)
        {
            var assemblies = Assembly.GetExecutingAssembly();
            //var assemblies = AppDomain.CurrentDomain.GetAssemblies();
            services.AddMediatR(assemblies);
            services.AddAutoMapper(assemblies);
            //services.AddValidatorsFromAssembly(assemblies);
            return services;
        }
    }
}
