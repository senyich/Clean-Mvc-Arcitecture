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

namespace Auction.Web.ServiceExtension
{
    public static class ServiceBuilder
    {
        public static IServiceCollection AddRepositories(this IServiceCollection services)
        {     
            services.AddScoped<IDbRepository<AuctionEntity>, AuctionRepository>();
            services.AddScoped<IDbRepository<UserEntity>, UserRepository>();
            services.AddScoped<IDbRepository<ItemEntity>, GameRepository>();
            services.AddScoped<ILoggerRepository, LoggerDbRepository>();
            return services;
        }
        public static IServiceCollection AddDbContexts(this IServiceCollection services, ConfigurationManager config)
        {
            config.AddJsonFile("appsettings.json");
            string mainConnString = config.GetConnectionString("MainConnection")!;
            string logConnString = config.GetConnectionString("LogDbConnection")!;
            services.AddDbContext<AuctionContext>(option=>option
                                .UseNpgsql(mainConnString));
            services.AddDbContext<LoggerContext>(option=>option
                                .UseNpgsql(logConnString));
            return services;
        }
        public static IServiceCollection AddServices(this IServiceCollection services)
        {
            services.AddScoped<IAuctionValidationService, AuctionRepositoryValidationService>();
            services.AddScoped<IItemValidationService, ItemsRepositoryValidationService>();
            services.AddScoped<IUserValidationService, UserRepositoryValidationService>();
            services.AddScoped<IConverter<UserEntity, UserModel>, UserEntityConverterService>();
            services.AddScoped<IConverter<UserModel,UserEntity>, UserModelConverterService>();
            services.AddScoped<IConverter<AuctionModel,AuctionEntity>, AuctionModelConverterService>();
            services.AddScoped<IConverter<AuctionEntity, AuctionModel>, AuctionEntityConverterService>();
            services.AddScoped<IConverter<ItemEntity, ItemModel>, ItemEntityConverterService>();
            services.AddScoped<IConverter<ItemModel,ItemEntity>, GameModelConverterService>();
            services.AddScoped<IFileLogisticService, ImagesLogisticService>();
            services.AddScoped<ISecurityService, SecurityService>();
            services.AddScoped<IAuthService, UserAuthService>();
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
                        Encoding.UTF8.GetBytes("younkyounkyounkyounkyounkyounkyounkyounkyounk"))
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