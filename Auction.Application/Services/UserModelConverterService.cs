using Auction.Application.Abstractions;
using Auction.Domain.Entities;
using Auction.Domain.Models;

namespace Auction.Application.Services
{
    public class UserModelConverterService : IConverter<UserModel, UserEntity>
    {
        public async Task<UserEntity> ConvertAsync(UserModel model)
        {
            var user = new UserEntity();
            user.UserName = model.UserName;
            user.PasswordHash = model.PasswordHash;
            user.Balance = model.Balance;
            return user;
        }
    }
}

