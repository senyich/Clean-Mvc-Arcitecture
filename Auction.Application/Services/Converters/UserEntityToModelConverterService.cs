using OrderWebsite.Application.Abstractions;
using OrderWebsite.Domain.Entities;
using OrderWebsite.Domain.Models;

namespace OrderWebsite.Application.Services
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