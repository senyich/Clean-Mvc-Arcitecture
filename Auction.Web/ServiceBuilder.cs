using Microsoft.EntityFrameworkCore;
using Auction.Application.Abstractions;
using Auction.Application.Services;
using Auction.Infrastructure;
using Auction.Infrastructure.Repositories;
using Auction.Domain.Repositories.Abstraction;
using Auction.Domain.Entities;
using Auction.Domain.Models;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using Auction.Domain.Repositories;
using Microsoft.Extensions.DependencyInjection;

namespace Auction.Web.ServiceExtension
{
    public static class ServiceBuilder
    {
        public static ConfigurationManager AddConfigurationFile(this ConfigurationManager config, string filename)
        {
            config.AddJsonFile(filename);
            return config;
        }
        public static IServiceCollection AddRepositories(this IServiceCollection services)
        {     
            services.AddScoped<IDbRepository<OrderEntity>, AuctionRepository>();
            services.AddScoped<IDbRepository<ItemEntity>, ItemRepository>();
            services.AddScoped<IUserRepository, UserRepository>();
            services.AddScoped<ILoggerRepository, LoggerDbRepository>();
            return services;
        }
        public static IServiceCollection AddDbContexts(this IServiceCollection services, ConfigurationManager config)
        {
            string mainConnString = config.GetConnectionString("MainConnection")!;
            string logConnString = config.GetConnectionString("LogDbConnection")!;
            services.AddDbContext<AuctionContext>(option=>option
                                .UseNpgsql(mainConnString));
            services.AddDbContext<LoggerContext>(option=>option
                                .UseNpgsql(logConnString));
            return services;
        }
        public static IServiceCollection AddServices(this IServiceCollection services, ConfigurationManager config)
        {
            services.AddScoped<IConfigurationManager>(c=>config);
            services.AddScoped<IAuctionValidationService, AuctionRepositoryValidationService>();
            services.AddScoped<IItemValidationService, ItemsRepositoryValidationService>();
            services.AddScoped<IUserValidationService, UserRepositoryValidationService>();
            services.AddScoped<IConverter<UserEntity, UserModel>, UserEntityToModelConverterService>();
            services.AddScoped<IConverter<UserModel,UserEntity>, UserModelToEntityConverterService>();
            services.AddScoped<IConverter<OrderModel,OrderEntity>, OrderModelToEntityConverterService>();
            services.AddScoped<IConverter<OrderEntity, OrderModel>, OrderEntityToModelConverterService>();
            services.AddScoped<IConverter<ItemEntity, ItemModel>, ItemEntityToModelConverterService>();
            services.AddScoped<IConverter<ItemModel,ItemEntity>, ItemModelToEntityConverterService>();
            services.AddScoped<IFileLogisticService, ImagesLogisticService>();
            services.AddScoped<ISecurityService, SecurityService>();
            services.AddScoped<IAuthService, UserAuthService>();
            services.AddScoped<ILoggerService, DatabaseLoggerService>();
            services.AddControllersWithViews();
            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme).AddJwtBearer(JwtBearerDefaults.AuthenticationScheme, options=>
            {
                options.TokenValidationParameters = new()
                {
                    ValidateIssuer = false,
                    ValidateAudience = false,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(
                        Encoding.UTF8.GetBytes(config["JwtTokenCode"]!))
                };
                options.Events = new JwtBearerEvents()
                {
                    OnMessageReceived = context =>
                    {
                        context.Token = context.Request.Cookies["myToken"];
                        return Task.CompletedTask;
                    }
                };
            });
            services.AddAuthorization();
            return services;
        }
    }
}