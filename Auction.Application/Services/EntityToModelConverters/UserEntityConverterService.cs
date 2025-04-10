using Auction.Domain.Abstractions;
using Auction.Domain.Entities;
using Auction.Domain.Enums;
using Auction.Domain.Models;

namespace Auction.Application.Services
{
    public class UserEntityConverterService : IConverter<UserEntity, UserModel>
    {
        private ILoggerService logger;
        public UserEntityConverterService(ILoggerService logger)
        {
            this.logger = logger;
        }
        public async Task<UserModel> Convert(UserEntity obj)
        {
            var user = UserModel.Create(obj.Id, obj.UserName, obj.PasswordHash);
            if(user.model!=null)
            {
                await logger.LogAsync("UserEntityConverter","Успешная конвертация",LogState.Success);
                return user.model;
            }
            else
            { 
                await logger.LogAsync("UserEntityConverter",$"Ошибка конвертации: {user.error}",LogState.Error);
                throw new ArgumentNullException(user.error);
            }
        }
    }
}