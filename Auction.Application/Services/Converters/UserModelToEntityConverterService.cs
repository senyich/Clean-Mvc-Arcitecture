using OrderWebsite.Application.Abstractions;
using OrderWebsite.Domain.Entities;
using OrderWebsite.Domain.Models;

namespace OrderWebsite.Application.Services
{
    public class UserModelToEntityConverterService : IConverter<UserModel, UserEntity>
    {
        public async Task<UserEntity> ConvertAsync(UserModel userModel)
        {
            var userEntity = new UserEntity();
            userEntity.UserName = userModel.UserName;
            userEntity.PasswordHash = userModel.PasswordHash;
            userEntity.Balance = userModel.Balance;
            return userEntity;
        }
    }
}

