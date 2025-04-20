using Auction.Application.Abstractions;
using Auction.Domain.Entities;
using Auction.Domain.Models;

namespace Auction.Application.Services
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

