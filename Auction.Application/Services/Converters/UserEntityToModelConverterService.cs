using Auction.Application.Abstractions;
using Auction.Domain.Entities;
using Auction.Domain.Models;

namespace Auction.Application.Services
{
    public class UserEntityToModelConverterService : IConverter<UserEntity, UserModel>
    {
        public async Task<UserModel> ConvertAsync(UserEntity userEntity)
        {
            (UserModel model, string error) userModel = UserModel.Create(userEntity.Id, userEntity.UserName, userEntity.PasswordHash, userEntity.Balance);
            if(userModel.model!=null)
                return userModel.model;
            else
                throw new ArgumentNullException(userModel.error);
        }
    }
}