using Microsoft.EntityFrameworkCore;
using OrderWebsite.Application.Abstractions;
using OrderWebsite.Application.Services;
using OrderWebsite.Infrastructure;
using OrderWebsite.Infrastructure.Repositories;
using OrderWebsite.Domain.Repositories.Abstraction;
using OrderWebsite.Domain.Entities;
using OrderWebsite.Domain.Models;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using OrderWebsite.Domain.Repositories;
using System.Text;

namespace OrderWebsite.Web.Extensions
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
            services.AddScoped<IOrderRepository, OrderRepository>();
            services.AddScoped<IItemRepository, ItemRepository>();
            services.AddScoped<IUserRepository, UserRepository>();
            services.AddScoped<ILoggerRepository, LoggerDbRepository>();
            return services;
        }
        public static IServiceCollection AddDbContexts(this IServiceCollection services, ConfigurationManager config)
        {
            string ordersConnString = config.GetConnectionString("OrdersDb")!;
            string logConnString = config.GetConnectionString("LogsDb")!;
            string itemsConnString = config.GetConnectionString("ItemsDb")!;
            services.AddDbContext<TradingExchangeContext>(option=>option
                                .UseNpgsql(ordersConnString));
            services.AddDbContext<LoggerContext>(option=>option
                                .UseNpgsql(logConnString));    
            services.AddDbContext<ItemsContext>(option=>option
                                .UseNpgsql(itemsConnString));
            return services;
        }
        public static IServiceCollection AddServices(this IServiceCollection services, ConfigurationManager config)
        {
            services.AddScoped<IConfigurationManager>(c=>config);
            services.AddScoped<IOrderValidationService, OrderRepositoryValidationService>();
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