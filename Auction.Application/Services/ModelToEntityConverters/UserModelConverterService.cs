using Auction.Application.Abstractions;
using Auction.Domain.Entities;
using Auction.Domain.Models;

namespace Auction.Application.Services
{
    public class UserModelConverterService : IConverter<UserModel, UserEntity>
    {
        public async Task<UserEntity> Convert(UserModel model)
        {
            var entity = new UserEntity();
            entity.UserName = model.UserName;
            entity.PasswordHash = model.PasswordHash;
            return entity;
        }
    }
}

