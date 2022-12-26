using AspNetCoreRateLimit;
using FF.Api.Services;
using FF.Core.Interface;
using FF.Core.Models;
using FF.Infrastructure.Data;
using FF.Infrastructure.RateLimit;
using FlippingFlips.Infrastructure.Service;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using System.Text;

namespace FF.Infrastructure
{
    public static class ServiceExtensions
    {
        /// <summary>
        /// Add FF.Infrasructure services
        /// </summary>
        /// <param name="services"></param>
        /// <returns></returns>
        public static IServiceCollection AddInfrastructure(this IServiceCollection services)
        {
            services.AddSingleton<IEmailSender, FlipsMail>();
            services.AddScoped<IFlipsService, FlipsService>();
            services.AddScoped<IFileService, FlipsLocalFileService>();
            return services;
        }

        /// <summary>
        /// Create <see cref="ApplicationDbContext"/> SQlite connection
        /// </summary>
        /// <param name="services"></param>
        /// <param name="configuration"></param>
        /// <returns></returns>
        public static IServiceCollection AddSQliteDbContext(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContext<ApplicationDbContext>(o =>
                o.UseSqlite(configuration.GetConnectionString("DefaultConnection"), b => b.MigrationsAssembly("FF.Infrastructure"))
            );

            services.AddScoped<IRepository, ApplicationDbContext>();

            return services;
        }

        /// <summary>
        /// AddDefaultIdentity and token providers
        /// </summary>
        /// <param name="services"></param>
        /// <param name="configuration"></param>
        /// <returns></returns>
        public static IServiceCollection AddDefaultIdentity(this IServiceCollection services, IConfiguration configuration)
        {
            //roles and requires user confirmed email before sign in. Use appSettings to change this EmailConfirmUsers
            bool.TryParse(configuration["RequireConfirmedAccount"], out bool emailConfirmUser);
            services.AddDefaultIdentity<ApplicationUser>(options => options.SignIn.RequireConfirmedAccount = emailConfirmUser)
                .AddRoles<IdentityRole>()
                .AddEntityFrameworkStores<ApplicationDbContext>()
                .AddDefaultTokenProviders();

            return services;
        }

        /// <summary>
        /// Adds Identity server and JWT access. Make sure JWT options are set in appsettings
        /// </summary>
        /// <param name="services"></param>
        /// <param name="configuration"></param>
        /// <returns></returns>
        public static IServiceCollection AddIdentityServerWithJWT(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddIdentityServer()
            .AddApiAuthorization<ApplicationUser, ApplicationDbContext>(options =>
            {
                options.IdentityResources["openid"].UserClaims.Add("role");
                if (options.ApiResources?.Count > 0)
                    options.ApiResources?.Single()?.UserClaims?.Add("role");
            });

            //JWT - Add Bearer tokens auth. This isn't default authentication
            bool.TryParse(configuration["Jwt:ValidateIssuer"], out bool validateIssuer);
            bool.TryParse(configuration["Jwt:ValidateAudience"], out bool validateAudience);
            services.AddAuthentication().AddJwtBearer(options =>
            {
                options.SaveToken = true;
                options.RequireHttpsMetadata = false;
                options.TokenValidationParameters = new TokenValidationParameters()
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidAudience = configuration["Jwt:Audience"],
                    ValidIssuer = configuration["Jwt:Issuer"],
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["Jwt:Key"]))
                };
            });
            return services;
        }

        /// <summary>
        /// Adds policies for API controller access and external login providers
        /// </summary>
        /// <param name="services"></param>
        /// <param name="configuration"></param>
        /// <returns></returns>
        public static IServiceCollection AddAuthorizationAndAuthentication(this IServiceCollection services, IConfiguration configuration)
        {
            //Add policies so the bearer and the web application can acess controllers
            services.AddAuthorization(options =>
            {
                var authPolicies = new AuthorizationPolicyBuilder("Bearer", "Identity.Application")
                    .RequireAuthenticatedUser()
                    .Build();
                options.AddPolicy("ApiAndWebPolicy", authPolicies);
            });

            //add google logins
            services.AddAuthentication().AddGoogle(googleOptions =>
            {
                googleOptions.ClientId = configuration["Authentication:Google:ClientId"];
                googleOptions.ClientSecret = configuration["Authentication:Google:ClientSecret"];
            });

            return services;
        }

        /// <summary>
        /// Adds client Rate Limiting use ClientRateLimiting appsettings.json
        /// </summary>
        /// <param name="services"></param>
        /// <param name="configuration"></param>
        /// <returns></returns>
        public static IServiceCollection AddRateLimiting(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddOptions();
            services.Configure<ClientRateLimitOptions>(configuration.GetSection("ClientRateLimiting"));
            services.AddMemoryCache();
            services.AddSingleton<IClientPolicyStore, MemoryCacheClientPolicyStore>();
            services.AddSingleton<IRateLimitCounterStore, MemoryCacheRateLimitCounterStore>();
            services.AddSingleton<IProcessingStrategy, AsyncKeyLockProcessingStrategy>();
            services.AddSingleton<IRateLimitConfiguration, FlipRateLimitConfig>();

            return services;
        }
    }
}
